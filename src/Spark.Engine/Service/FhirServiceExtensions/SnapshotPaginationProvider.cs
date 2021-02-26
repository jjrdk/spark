namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;
    using Spark.Engine.Core;
    using Spark.Engine.Extensions;
    using Spark.Engine.Store.Interfaces;

    public class SnapshotPaginationProvider : ISnapshotPaginationProvider, ISnapshotPagination
    {
        private readonly IFhirStore _fhirStore;
        private readonly ITransfer _transfer;
        private readonly ILocalhost _localhost;
        private readonly ISnapshotPaginationCalculator _snapshotPaginationCalculator;
        private Snapshot _snapshot;

        public SnapshotPaginationProvider(
            IFhirStore fhirStore,
            ITransfer transfer,
            ILocalhost localhost,
            ISnapshotPaginationCalculator snapshotPaginationCalculator)
        {
            this._fhirStore = fhirStore;
            this._transfer = transfer;
            this._localhost = localhost;
            _snapshotPaginationCalculator = snapshotPaginationCalculator;
        }

        public ISnapshotPagination StartPagination(Snapshot snapshot)
        {
            this._snapshot = snapshot;
            return this;
        }

        public async Task<Bundle> GetPage(int? index = null, Action<Entry> transformElement = null)
        {
            if (_snapshot == null)
            {
                throw Error.NotFound("There is no paged snapshot");
            }

            return !_snapshot.InRange(index ?? 0)
                ? throw Error.NotFound(
                    "The specified index lies outside the range of available results ({0}) in snapshot {1}",
                    _snapshot.Keys.Count(),
                    _snapshot.Id)
                : await CreateBundleAsync(index).ConfigureAwait(false);
        }

        private async Task<Bundle> CreateBundleAsync(int? start = null)
        {
            var bundle = new Bundle {Type = _snapshot.Type, Total = _snapshot.Count, Id = Guid.NewGuid().ToString()};

            var keys = _snapshotPaginationCalculator.GetKeysForPage(_snapshot, start).ToList();
            var entries = (await _fhirStore.Get(keys).ConfigureAwait(false)).ToList();
            if (_snapshot.SortBy != null)
            {
                entries = entries.Select(e => new {Entry = e, Index = keys.IndexOf(e.Key)})
                    .OrderBy(e => e.Index)
                    .Select(e => e.Entry)
                    .ToList();
            }

            var included = await GetIncludesRecursiveForAsync(entries, _snapshot.Includes).ConfigureAwait(false);
            entries.Append(included);

            _transfer.Externalize(entries);
            bundle.Append(entries);
            BuildLinks(bundle, start);

            return bundle;
        }


        private async Task<IList<Entry>> GetIncludesRecursiveForAsync(
            IList<Entry> entries,
            ICollection<string> includes)
        {
            IList<Entry> included = new List<Entry>();

            var latest = await GetIncludesForAsync(entries, includes).ConfigureAwait(false);
            int previousCount;
            do
            {
                previousCount = included.Count;
                included.AppendDistinct(latest);
                latest = await GetIncludesForAsync(latest, includes).ConfigureAwait(false);
            }
            while (included.Count > previousCount);

            return included;
        }

        private async Task<IList<Entry>> GetIncludesForAsync(IList<Entry> entries, IEnumerable<string> includes)
        {
            if (includes == null)
            {
                return new List<Entry>();
            }

            var paths = includes.SelectMany(IncludeToPath);
            IList<IKey> identifiers = entries.GetResources()
                .GetReferences(paths)
                .Distinct()
                .Select(k => (IKey) Key.ParseOperationPath(k))
                .ToList();

            IList<Entry> result = (await _fhirStore.Get(identifiers).ConfigureAwait(false)).ToList();

            return result;
        }

        private void BuildLinks(Bundle bundle, int? start = null)
        {
            bundle.SelfLink = start == null
                ? _localhost.Absolute(new Uri(_snapshot.FeedSelfLink, UriKind.RelativeOrAbsolute))
                : BuildSnapshotPageLink(0);
            bundle.FirstLink = BuildSnapshotPageLink(0);
            bundle.LastLink = BuildSnapshotPageLink(_snapshotPaginationCalculator.GetIndexForLastPage(_snapshot));

            var previousPageIndex = _snapshotPaginationCalculator.GetIndexForPreviousPage(_snapshot, start);
            if (previousPageIndex != null)
            {
                bundle.PreviousLink = BuildSnapshotPageLink(previousPageIndex);
            }

            var nextPageIndex = _snapshotPaginationCalculator.GetIndexForNextPage(_snapshot, start);
            if (nextPageIndex != null)
            {
                bundle.NextLink = BuildSnapshotPageLink(nextPageIndex);
            }
        }

        private Uri BuildSnapshotPageLink(int? snapshotIndex)
        {
            if (snapshotIndex == null)
            {
                return null;
            }

            Uri baseurl;
            if (string.IsNullOrEmpty(_snapshot.Id) == false)
            {
                //baseUrl for statefull pagination
                baseurl = new Uri(_localhost.DefaultBase + "/" + FhirRestOp.SNAPSHOT).AddParam(
                    FhirParameter.SNAPSHOT_ID,
                    _snapshot.Id);
            }
            else
            {
                //baseUrl for stateless pagination
                baseurl = new Uri(_snapshot.FeedSelfLink);
            }

            return baseurl.AddParam(FhirParameter.SNAPSHOT_INDEX, snapshotIndex.ToString());
        }

        private IEnumerable<string> IncludeToPath(string include)
        {
            var split = include.Split(':');
            var resource = split.FirstOrDefault();
            var paramname = split.Skip(1).FirstOrDefault();
            var param = ModelInfo.SearchParameters.FirstOrDefault(p => p.Resource == resource && p.Name == paramname);
            return param != null ? param.Path : Enumerable.Empty<string>();
        }
    }
}
