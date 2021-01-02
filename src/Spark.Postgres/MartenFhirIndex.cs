namespace Spark.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Interfaces;
    using Engine.Model;
    using Engine.Search.ValueExpressionTypes;
    using Engine.Store.Interfaces;
    using Hl7.Fhir.Rest;
    using Marten;
    using Microsoft.Extensions.Logging;

    public class MartenFhirIndex : IFhirIndex, IIndexStore
    {
        private readonly ILogger<MartenFhirIndex> _logger;
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenFhirIndex(ILogger<MartenFhirIndex> logger, Func<IDocumentSession> sessionFunc)
        {
            _logger = logger;
            _sessionFunc = sessionFunc;
        }

        /// <inheritdoc />
        async Task IIndexStore.Save(IndexValue indexValue)
        {
            using var session = _sessionFunc();
            var values = indexValue.IndexValues().ToArray();
            var id = (StringValue)values.First(x => x.Name == "internal_forResource").Values[0];
            var entry = await session.LoadAsync<IndexEntry>(id.Value).ConfigureAwait(false);
            if (entry == null)
            {
                entry = new IndexEntry(id.Value, new Dictionary<string, object>());
            }
            else
            {
                entry.Values.Clear();
            }

            foreach (var value in values)
            {
                var array = value.Values.Where(x => x != null).Select(GetValue).ToArray();
                var o = array.Length == 1 ? array[0] : array;
                entry.Values.Add(value.Name, o);
            }

            session.Store(entry);

            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        private static object GetValue(Expression expression)
        {
            return expression switch
            {
                IndexValue indexValue => indexValue.Values.Select(GetValue).ToArray(),
                CompositeValue compositeValue =>
                    compositeValue.Components.OfType<IndexValue>().All(x => x.Name == "code")
                        ? compositeValue.Components.OfType<IndexValue>()
                            .SelectMany(x => x.Values.Select(GetValue))
                            .First()
                        : compositeValue.Components.OfType<IndexValue>()
                            .ToDictionary(
                                component => component.Name,
                                component => component.Values.Select(GetValue).ToArray()),
                _ => expression.ToString()
            };
        }

        /// <inheritdoc />
        async Task IIndexStore.Delete(Entry entry)
        {
            using var session = _sessionFunc();
            session.Delete<IndexEntry>(entry.Key.ToStorageKey());
            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        Task IFhirIndex.Clean()
        {
            _logger.LogDebug("Clean requested");
            return Task.CompletedTask;
        }

        Task IIndexStore.Clean()
        {
            _logger.LogDebug("Clean requested");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<SearchResults> Search(string resource, SearchParams searchCommand)
        {
            _logger.LogDebug($"{resource} search requested with {searchCommand.ToUriParamList().ToQueryString()}");
            var resources = await GetIndexValues(resource, searchCommand).ConfigureAwait(false);

            var count = resources.Length;

            if (searchCommand.Count.HasValue && searchCommand.Count.Value > 0)
            {
                resources = resources.Take(searchCommand.Count.Value).ToArray();
            }

            var keys = resources.ToList();
            var results = new SearchResults
            {
                MatchCount = count,
                UsedCriteria = searchCommand.Parameters.Select(t => Criterium.Parse(t.Item1, t.Item2)).ToList()
            };

            results.AddRange(keys);

            return results;
        }

        private async Task<string[]> GetIndexValues(string resource, SearchParams searchCommand)
        {
            var criteria = searchCommand.Parameters.Select(t => Criterium.Parse(t.Item1, t.Item2));
            using var session = _sessionFunc();
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat(@"where data -> 'Values' -> 'internal_resource' = '{0}'", resource);
            queryBuilder = criteria.Aggregate(
                queryBuilder,
                (sb, c) => sb.AppendFormat(@" and data -> 'Values' {0}", GetComparison(c)));

            var sql = queryBuilder.ToString();
            _logger.LogDebug($"Executing query: {sql}");
            var result = await session.QueryAsync<IndexEntry>(sql).ConfigureAwait(false);

            return result.Select(iv => iv.Values.First(v => v.Key == "internal_id").Value as string).Distinct().ToArray();
        }

        private static string GetComparison(Criterium criterium)
        {
            return criterium.Operator switch
            {
                Operator.EQ => $"->> '{criterium.ParamName}' = '{GetValue(criterium.Operand)}'",
                Operator.LT => $"->> '{criterium.ParamName}' < '{GetValue(criterium.Operand)}'",
                Operator.LTE => $"->> '{criterium.ParamName}' <= '{GetValue(criterium.Operand)}'",
                Operator.APPROX => $"->> '{criterium.ParamName}' = '{GetValue(criterium.Operand)}'",
                Operator.GTE => $"->> '{criterium.ParamName}' >= '{GetValue(criterium.Operand)}'",
                Operator.GT => $"->> '{criterium.ParamName}' > '{GetValue(criterium.Operand)}'",
                Operator.ISNULL => $"->'{criterium.ParamName}' IS NULL",
                Operator.NOTNULL => $"-> '{criterium.ParamName}' IS NOT NULL",
                Operator.IN => $"-> '{criterium.ParamName}' ? {GetValue(criterium.Operand)}",
                Operator.CHAIN => $"-> {criterium.ParamName} is null",
                Operator.NOT_EQUAL => $"->> '{criterium.ParamName}' != '{GetValue(criterium.Operand)}'",
                Operator.STARTS_AFTER => $"-> '{criterium.ParamName}' -> 'start' ->> 0 > '{GetValue(criterium.Operand)}'",
                Operator.ENDS_BEFORE => $"-> '{criterium.ParamName}' -> 'end' ->> 0 < '{GetValue(criterium.Operand)}'",
                _ => throw new ArgumentOutOfRangeException(nameof(criterium.Operator), "Invalid operator")
            };
        }

        /// <inheritdoc />
        public async Task<Key> FindSingle(string resource, SearchParams searchCommand)
        {
            _logger.LogDebug($"Find single {resource} key");
            var entries = await GetIndexValues(resource, searchCommand).ConfigureAwait(false);

            return Key.ParseOperationPath(entries.FirstOrDefault());
        }

        /// <inheritdoc />
        public Task<SearchResults> GetReverseIncludes(IList<IKey> keys, IList<string> revIncludes)
        {
            throw new NotImplementedException();
        }
    }
}
