﻿/*
 * Copyright (c) 2021, Incendi (info@incendi.no) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/spark/stu3/master/LICENSE
 */

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using Hl7.Fhir.Model;
    using Spark.Engine.Core;
    using Spark.Engine.Extensions;
    using Spark.Engine.Store.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SnapshotPaginationService : ISnapshotPagination
    {
        private readonly IFhirStore _fhirStore;
        private readonly ITransfer _transfer;
        private readonly ILocalhost _localhost;
        private readonly ISnapshotPaginationCalculator _snapshotPaginationCalculator;
        private readonly Snapshot _snapshot;

        public SnapshotPaginationService(
            IFhirStore fhirStore,
            ITransfer transfer,
            ILocalhost localhost,
            ISnapshotPaginationCalculator snapshotPaginationCalculator,
            Snapshot snapshot)
        {
            _fhirStore = fhirStore;
            _transfer = transfer;
            _localhost = localhost;
            _snapshotPaginationCalculator = snapshotPaginationCalculator;
            _snapshot = snapshot;
        }

        public async Task<Bundle> GetPage(int? index = null, Action<Entry> transformElement = null)
        {
            if (_snapshot == null)
                throw Error.NotFound("There is no paged snapshot");

            if (!_snapshot.InRange(index ?? 0))
            {
                throw Error.NotFound(
                    "The specified index lies outside the range of available results ({0}) in snapshot {1}",
                    _snapshot.Keys.Count(),
                    _snapshot.Id);
            }

            return await CreateBundle(index).ConfigureAwait(false);
        }
        
        private async Task<Bundle> CreateBundle(int? start = null)
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

            var included = await GetIncludesRecursiveFor(entries, _snapshot.Includes).ConfigureAwait(false);
            entries.Append(included);

            _transfer.Externalize(entries);
            bundle.Append(entries);
            BuildLinks(bundle, start);

            return bundle;
        }

        private async Task<IList<Entry>> GetIncludesRecursiveFor(IList<Entry> entries, IEnumerable<string> includes)
        {
            IList<Entry> included = new List<Entry>();

            var latest = await GetIncludesFor(entries, includes).ConfigureAwait(false);
            int previouscount;
            do
            {
                previouscount = included.Count;
                included.AppendDistinct(latest);
                latest = await GetIncludesFor(latest, includes).ConfigureAwait(false);
            }
            while (included.Count > previouscount);

            return included;
        }

        private async Task<IList<Entry>> GetIncludesFor(IList<Entry> entries, IEnumerable<string> includes)
        {
            if (includes == null) return new List<Entry>();

            var paths = includes.SelectMany(i => IncludeToPath(i));
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
                return null;

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

        private static IEnumerable<string> IncludeToPath(string include)
        {
            var _include = include.Split(':');
            var resource = _include.FirstOrDefault();
            var paramname = _include.Skip(1).FirstOrDefault();
            var param = ModelInfo.SearchParameters.FirstOrDefault(p => p.Resource == resource && p.Name == paramname);
            if (param != null)
            {
                return param.Path;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
