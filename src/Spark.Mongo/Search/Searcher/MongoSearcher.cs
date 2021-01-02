﻿/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

//using Hl7.Fhir.Support;
using SM = Spark.Engine.Search.Model;

namespace Spark.Mongo.Search.Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Search;
    using Engine.Search.ValueExpressionTypes;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Infrastructure;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Modifier = Common.Modifier;
    using SearchParamType = Hl7.Fhir.Model.SearchParamType;

    public class MongoSearcher
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly ILocalhost _localhost;
        private readonly IFhirModel _fhirModel;
        private readonly IReferenceNormalizationService _referenceNormalizationService;

        public MongoSearcher(
            MongoIndexStore mongoIndexStore,
            ILocalhost localhost,
            IFhirModel fhirModel,
            IReferenceNormalizationService referenceNormalizationService = null)
        {
            _collection = mongoIndexStore.Collection;
            _localhost = localhost;
            _fhirModel = fhirModel;
            _referenceNormalizationService = referenceNormalizationService;
        }

        private async Task<List<BsonValue>> CollectKeys(FilterDefinition<BsonDocument> query)
        {
            var cursor = (await _collection.FindAsync(
                    query,
                    new FindOptions<BsonDocument>
                    {
                        Projection = Builders<BsonDocument>.Projection.Include(InternalField.ID)
                    })
                .ConfigureAwait(false)).ToEnumerable();

            return cursor.Select(doc => doc.GetValue(InternalField.ID)).ToList();
        }

        private async Task<List<BsonValue>> CollectSelfLinks(
            FilterDefinition<BsonDocument> query,
            SortDefinition<BsonDocument> sortBy)
        {
            var cursor = await _collection.FindAsync(
                    query,
                    new FindOptions<BsonDocument>
                    {
                        Sort = sortBy, Projection = Builders<BsonDocument>.Projection.Include(InternalField.SELFLINK)
                    })
                .ConfigureAwait(false);

            return cursor.ToEnumerable().Select(doc => doc.GetValue(InternalField.SELFLINK)).ToList();
        }

        private SearchResults KeysToSearchResults(IEnumerable<BsonValue> keys)
        {
            var results = new SearchResults();

            if (keys.Any())
            {
                var cursor = _collection.Find(Builders<BsonDocument>.Filter.In(InternalField.ID, keys))
                    .Project(Builders<BsonDocument>.Projection.Include(InternalField.SELFLINK))
                    .ToEnumerable();

                foreach (var document in cursor)
                {
                    var id = document.GetValue(InternalField.SELFLINK).ToString();
                    //Uri rid = new Uri(id, UriKind.Relative); // NB. these MUST be relative paths. If not, the data at time of input was wrong
                    results.Add(id);
                }

                results.MatchCount = results.Count;
            }

            return results;
        }

        private Task<List<BsonValue>> CollectKeys(string resourceType, IEnumerable<Criterium> criteria, int level = 0)
        {
            return CollectKeys(resourceType, criteria, null, level);
        }

        private async Task<List<BsonValue>> CollectKeys(
            string resourceType,
            IEnumerable<Criterium> criteria,
            SearchResults results,
            int level)
        {
            var closedCriteria =
                await CloseChainedCriteria(resourceType, criteria, results, level).ConfigureAwait(false);

            //All chained criteria are 'closed' or 'rolled up' to something like subject IN (id1, id2, id3), so now we AND them with the rest of the criteria.
            var resultQuery = CreateMongoQuery(resourceType, results, level, closedCriteria);

            return await CollectKeys(resultQuery).ConfigureAwait(false);
        }

        private async Task<List<BsonValue>> CollectSelfLinks(string resourceType, IEnumerable<Criterium> criteria, SearchResults results, int level, IList<(string, SortOrder)> sortItems)
        {
            var closedCriteria =
                await CloseChainedCriteria(resourceType, criteria, results, level).ConfigureAwait(false);

            //All chained criteria are 'closed' or 'rolled up' to something like subject IN (id1, id2, id3), so now we AND them with the rest of the criteria.
            var resultQuery = CreateMongoQuery(resourceType, results, level, closedCriteria);
            var sortBy = CreateSortBy(sortItems);
            return await CollectSelfLinks(resultQuery, sortBy).ConfigureAwait(false);
        }

        private static SortDefinition<BsonDocument> CreateSortBy(IList<(string, SortOrder)> sortItems)
        {
            if (sortItems.Any() == false)
                return null;

            SortDefinition<BsonDocument> sortDefinition;
            var first = sortItems.FirstOrDefault();
            if (first.Item2 == SortOrder.Ascending)
            {
                sortDefinition = Builders<BsonDocument>.Sort.Ascending(first.Item1);
            }
            else
            {
                sortDefinition = Builders<BsonDocument>.Sort.Descending(first.Item1);
            }

            sortItems.Remove(first);
            foreach (var sortItem in sortItems)
            {
                if (sortItem.Item2 == SortOrder.Ascending)
                {
                    sortDefinition = sortDefinition.Ascending(sortItem.Item1);
                }
                else
                {
                    sortDefinition = sortDefinition.Descending(sortItem.Item1);
                }
            }
            return sortDefinition;

            //return sortItems.Aggregate(
            //    sortDefinition,
            //    (current, sortItem) => sortItem.Item2 == SortOrder.Ascending
            //        ? current.Ascending(sortItem.Item1)
            //        : current.Descending(sortItem.Item1));
        }

        private static FilterDefinition<BsonDocument> CreateMongoQuery(
            string resourceType,
            SearchResults results,
            int level,
            Dictionary<Criterium, Criterium> closedCriteria)
        {
            var resultQuery = CriteriaMongoExtensions.ResourceFilter(resourceType, level);
            if (closedCriteria.Count > 0)
            {
                var criteriaQueries = new List<FilterDefinition<BsonDocument>>();
                foreach (var crit in closedCriteria)
                {
                    if (crit.Value != null)
                    {
                        try
                        {
                            criteriaQueries.Add(crit.Value.ToFilter(resourceType));
                        }
                        catch (ArgumentException ex)
                        {
                            if (results == null) throw; //The exception *will* be caught on the highest level.
                            results.AddIssue(
                                $"Parameter [{crit.Key}] was ignored for the reason: {ex.Message}.",
                                OperationOutcome.IssueSeverity.Warning);
                            results.UsedCriteria.Remove(crit.Key);
                        }
                    }
                }

                if (criteriaQueries.Count > 0)
                {
                    var criteriaQuery = Builders<BsonDocument>.Filter.And(criteriaQueries);
                    resultQuery = Builders<BsonDocument>.Filter.And(resultQuery, criteriaQuery);
                }
            }

            return resultQuery;
        }

        private async Task<Dictionary<Criterium, Criterium>> CloseChainedCriteria(
            string resourceType,
            IEnumerable<Criterium> criteria,
            SearchResults results,
            int level)
        {
            //Mapping of original criterium and closed criterium, the former to be able to exclude it if it errors later on.
            var closedCriteria = new Dictionary<Criterium, Criterium>();
            foreach (var c in criteria)
            {
                if (c.Operator == Operator.CHAIN)
                {
                    try
                    {
                        var closeCriterium = await CloseCriterium(c, resourceType, level).ConfigureAwait(false);
                        closedCriteria.Add(c.Clone(), closeCriterium);
                        //CK: We don't pass the SearchResults on to the (recursive) CloseCriterium. We catch any exceptions only on the highest level.
                    }
                    catch (ArgumentException ex)
                    {
                        if (results == null) throw; //The exception *will* be caught on the highest level.
                        results.AddIssue(
                            $"Parameter [{c}] was ignored for the reason: {ex.Message}.",
                            OperationOutcome.IssueSeverity.Warning);
                        results.UsedCriteria.Remove(c);
                    }
                }
                else
                {
                    //If it is not a chained criterium, we don't need to 'close' it, so it is said to be 'closed' already.
                    closedCriteria.Add(c, c);
                }
            }

            return closedCriteria;
        }

        /// <summary>
        /// CloseCriterium("patient.name=\"Teun\"") -> "patient IN (id1,id2)"
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="crit"></param>
        /// <returns></returns>
        private async Task<Criterium> CloseCriterium(Criterium crit, string resourceType, int level)
        {
            var targeted = crit.GetTargetedReferenceTypes(resourceType);
            var allKeys = new List<string>();
            var errors = new List<Exception>();
            foreach (var target in targeted)
            {
                try
                {
                    var innerCriterium = (Criterium) crit.Operand;
                    var keys = await CollectKeys(target, new List<Criterium> {innerCriterium}, ++level)
                        .ConfigureAwait(false); //Recursive call to CollectKeys!
                    allKeys.AddRange(keys.Select(k => k.ToString()));
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            if (errors.Count == targeted.Count)
            {
                //It is possible that some of the targets don't support the current parameter. But if none do, there is a serious problem.
                throw new ArgumentException(
                    $"None of the possible target resources support querying for parameter {crit.ParamName}");
            }

            crit.Operator = Operator.IN;
            crit.Operand = new ChoiceValue(allKeys.Select(k => new UntypedValue(k)));
            return crit;
        }

        /// <summary>
        /// Change something like Condition/subject:Patient=Patient/10014
        /// to Condition/subject:Patient.internal_id=Patient/10014, so it is correctly handled as a chained parameter,
        /// including the filtering on the type in the modifier (if any).
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        private List<Criterium> NormalizeNonChainedReferenceCriteria(
            List<Criterium> criteria,
            string resourceType,
            SearchSettings searchSettings)
        {
            var result = new List<Criterium>();

            foreach (var crit in criteria)
            {
                var critSp = crit.FindSearchParamDefinition(resourceType);
                //                var critSp_ = _fhirModel.FindSearchParameter(resourceType, crit.ParamName); HIER VERDER: kunnen meerdere searchParameters zijn, hoewel dat alleen bij subcriteria van chains het geval is...
                if (critSp != null
                    && critSp.Type == SearchParamType.Reference
                    && crit.Operator != Operator.CHAIN
                    && crit.Modifier != Modifier.MISSING
                    && crit.Operand != null)
                {
                    if (_referenceNormalizationService != null
                        && searchSettings.ShouldSkipReferenceCheck(resourceType, crit.ParamName))
                    {
                        var normalizedCriteria = _referenceNormalizationService.GetNormalizedReferenceCriteria(crit);
                        if (normalizedCriteria != null)
                        {
                            result.Add(normalizedCriteria);
                        }

                        continue;
                    }

                    var subCrit = new Criterium {Operator = crit.Operator};
                    var modifier = crit.Modifier;

                    //operand can be one of three things:
                    //1. just the id: 10014 (in the index as internal_justid), with no modifier
                    //2. just the id, but with a modifier that contains the type: Patient:10014
                    //3. full id: [http://localhost:xyz/fhir/]Patient/10014 (in the index as internal_id):
                    //  - might start with a host: http://localhost:xyz/fhir/Patient/100014
                    //  - the type in the modifier (if present) is no longer relevant
                    //And above that, you might have multiple identifiers with an IN operator. So we have to cater for that as well.
                    //Because we cannot express an OR construct in Criterium, we have choose one situation for all identifiers. We inspect the first, to determine which situation is appropriate.

                    //step 1: get the operand value, or - in the case of a Choice - the first operand value.
                    string operand = null;
                    if (crit.Operand is ChoiceValue)
                    {
                        var choiceOperand = (crit.Operand as ChoiceValue);
                        if (!choiceOperand.Choices.Any())
                        {
                            continue; //Choice operator without choices: ignore it.
                        }
                        else
                        {
                            operand = (choiceOperand.Choices.First() as UntypedValue).Value;
                        }
                    }
                    else
                    {
                        operand = (crit.Operand as UntypedValue).Value;
                    }

                    //step 2: determine which situation is accurate
                    var situation = 3;
                    if (!operand.Contains("/")) //Situation 1 or 2
                    {
                        if (string.IsNullOrWhiteSpace(modifier)
                        ) // no modifier, so no info about the referenced type at all
                        {
                            situation = 1;
                        }
                        else //modifier contains the referenced type
                        {
                            situation = 2;
                        }
                    }

                    //step 3: create a subcriterium appropriate for every situation.
                    switch (situation)
                    {
                        case 1:
                            subCrit.ParamName = InternalField.JUSTID;
                            subCrit.Operand = crit.Operand;
                            break;
                        case 2:
                            subCrit.ParamName = InternalField.ID;
                            if (crit.Operand is ChoiceValue)
                            {
                                subCrit.Operand = new ChoiceValue(
                                    (crit.Operand as ChoiceValue).Choices.Select(
                                        choice => new UntypedValue(modifier + "/" + (choice as UntypedValue).Value))
                                    .ToList());
                            }
                            else
                            {
                                subCrit.Operand = new UntypedValue(modifier + "/" + operand);
                            }

                            break;
                        default: //remove the base of the url if there is one and it matches this server
                            subCrit.ParamName = InternalField.ID;
                            if (crit.Operand is ChoiceValue)
                            {
                                subCrit.Operand = new ChoiceValue(
                                    (crit.Operand as ChoiceValue).Choices.Select(
                                        choice =>
                                        {
                                            Uri.TryCreate(
                                                (choice as UntypedValue).Value,
                                                UriKind.RelativeOrAbsolute,
                                                out var uriOperand);
                                            var refUri =
                                                _localhost.RemoveBase(
                                                    uriOperand); //Drop the first part if it points to our own server.
                                            return new UntypedValue(refUri.ToString().TrimStart(new char[] {'/'}));
                                        }));
                            }
                            else
                            {
                                Uri.TryCreate(operand, UriKind.RelativeOrAbsolute, out var uriOperand);
                                var refUri =
                                    _localhost.RemoveBase(
                                        uriOperand); //Drop the first part if it points to our own server.
                                subCrit.Operand = new UntypedValue(refUri.ToString().TrimStart(new char[] {'/'}));
                            }

                            break;
                    }

                    var superCrit = new Criterium
                    {
                        ParamName = crit.ParamName,
                        Modifier = crit.Modifier,
                        Operator = Operator.CHAIN,
                        Operand = subCrit
                    };
                    superCrit.SearchParameters.AddRange(crit.SearchParameters);

                    result.Add(superCrit);
                }
                else result.Add(crit);
            }

            return result;
        }

        public async Task<SearchResults> Search(
            string resourceType,
            SearchParams searchCommand,
            SearchSettings searchSettings = null)
        {
            if (searchSettings == null)
            {
                searchSettings = new SearchSettings();
            }

            var results = new SearchResults();

            var criteria = ParseCriteria(searchCommand, results);

            if (!results.HasErrors)
            {
                results.UsedCriteria = criteria.Select(c => c.Clone()).ToList();

                criteria = EnrichCriteriaWithSearchParameters(
                    _fhirModel.GetResourceTypeForResourceName(resourceType),
                    results);

                var normalizedCriteria = NormalizeNonChainedReferenceCriteria(criteria, resourceType, searchSettings);
                var normalizeSortCriteria = NormalizeSortItems(resourceType, searchCommand);

                var selfLinks = await CollectSelfLinks(
                        resourceType,
                        normalizedCriteria,
                        results,
                        0,
                        normalizeSortCriteria)
                    .ConfigureAwait(false);

                foreach (var selfLink in selfLinks)
                {
                    results.Add(selfLink.ToString());
                }

                results.MatchCount = selfLinks.Count;
            }

            return results;
        }

        private IList<(string, SortOrder)> NormalizeSortItems(string resourceType, SearchParams searchCommand)
        {
            var sortItems = searchCommand.Sort.Select(s => NormalizeSortItem(resourceType, s)).ToList();
            return sortItems;
        }


        private (string, SortOrder) NormalizeSortItem(string resourceType, (string, SortOrder) sortItem)
        {
            var definition =
                _fhirModel.FindSearchParameter(resourceType, sortItem.Item1)?.GetOriginalDefinition();

            if (definition?.Type == SearchParamType.Token)
            {
                return (sortItem.Item1 + ".code", sortItem.Item2);
            }

            if (definition?.Type == SearchParamType.Date)
            {
                return (sortItem.Item1 + ".start", sortItem.Item2);
            }

            if (definition?.Type == SearchParamType.Quantity)
            {
                return (sortItem.Item1 + ".value", sortItem.Item2);
            }

            return sortItem;
        }

        public async Task<SearchResults> GetReverseIncludes(IList<IKey> keys, IList<string> revIncludes)
        {
            BsonValue[] internal_ids = keys.Select(k => BsonString.Create($"{k.TypeName}/{k.ResourceId}")).ToArray();

            var results = new SearchResults();

            if (keys != null && revIncludes != null)
            {
                var riQueries = new List<FilterDefinition<BsonDocument>>();

                foreach (var revInclude in revIncludes)
                {
                    var ri = SM.ReverseInclude.Parse(revInclude);
                    if (!ri.SearchPath.Contains(".")
                    ) //for now, leave out support for chained revIncludes. There aren't that many anyway.
                    {
                        riQueries.Add(
                            Builders<BsonDocument>.Filter.And(
                                Builders<BsonDocument>.Filter.Eq(InternalField.RESOURCE, ri.ResourceType),
                                Builders<BsonDocument>.Filter.In(ri.SearchPath, internal_ids)));
                    }
                }

                if (riQueries.Count > 0)
                {
                    var revIncludeQuery = Builders<BsonDocument>.Filter.Or(riQueries);
                    var resultKeys = await CollectKeys(revIncludeQuery).ConfigureAwait(false);
                    results = KeysToSearchResults(resultKeys);
                }
            }

            return results;
        }

        private bool TryEnrichCriteriumWithSearchParameters(Criterium criterium, ResourceType resourceType)
        {
            var sp = _fhirModel.FindSearchParameter(resourceType, criterium.ParamName);
            if (sp == null)
            {
                return false;
            }

            var result = true;

            var spDef = sp.GetOriginalDefinition();

            if (spDef != null)
            {
                criterium.SearchParameters.Add(spDef);
            }

            if (criterium.Operator == Operator.CHAIN)
            {
                var subCrit = (Criterium) (criterium.Operand);
                var subCritResult = false;
                foreach (var targetType in criterium.SearchParameters.SelectMany(spd => spd.Target))
                {
                    //We're ok if at least one of the target types has this searchparameter.
                    subCritResult |= TryEnrichCriteriumWithSearchParameters(subCrit, targetType);
                }

                result &= subCritResult;
            }

            return result;
        }

        private List<Criterium> EnrichCriteriaWithSearchParameters(ResourceType resourceType, SearchResults results)
        {
            var result = new List<Criterium>();
            var notUsed = new List<Criterium>();
            foreach (var crit in results.UsedCriteria)
            {
                if (TryEnrichCriteriumWithSearchParameters(crit, resourceType))
                {
                    result.Add(crit);
                }
                else
                {
                    notUsed.Add(crit);
                    results.AddIssue(
                        $"Parameter with name {crit.ParamName} is not supported for resource type {resourceType}.",
                        OperationOutcome.IssueSeverity.Warning);
                }
            }

            results.UsedCriteria = results.UsedCriteria.Except(notUsed).ToList();

            return result;
        }

        //TODO: Delete, F.Query is obsolete.
        /*
        public SearchResults Search(F.Query query)
        {
            SearchResults results = new SearchResults();

            var criteria = parseCriteria(query, results);

            if (!results.HasErrors)
            {
                results.UsedCriteria = criteria;
                //TODO: ResourceType.ToString() sufficient, or need to use EnumMapping?
                var normalizedCriteria = NormalizeNonChainedReferenceCriteria(criteria, query.ResourceType.ToString());
                List<BsonValue> keys = CollectKeys(query.ResourceType.ToString(), normalizedCriteria, results);

                int numMatches = keys.Count();

                results.AddRange(KeysToSearchResults(keys));
                results.MatchCount = numMatches;
            }

            return results;
        }
        */

        private static List<Criterium> ParseCriteria(SearchParams searchCommand, SearchResults results)
        {
            var result = new List<Criterium>();
            foreach (var c in searchCommand.Parameters)
            {
                try
                {
                    result.Add(Criterium.Parse(c.Item1, c.Item2));
                }
                catch (Exception ex)
                {
                    results.AddIssue($"Could not parse parameter [{c}] for reason [{ex.Message}].");
                }
            }

            return result;
        }

        //TODO: Delete, F.Query is obsolete.
        /*
        private List<Criterium> parseCriteria(F.Query query, SearchResults results)
        {
            var result = new List<Criterium>();
            foreach (var c in query.Criteria)
            {
                try
                {
                    result.Add(Criterium.Parse(c));
                }
                catch (Exception ex)
                {
                    results.AddIssue(String.Format("Could not parse parameter [{0}] for reason [{1}].", c.ToString(), ex.Message));
                }
            }
            return result;
        }
         */
    }
}
