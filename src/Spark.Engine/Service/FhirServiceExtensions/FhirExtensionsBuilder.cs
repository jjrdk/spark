﻿namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Spark.Engine.Core;
    using Spark.Engine.Store.Interfaces;
    using Interfaces;
    using Spark.Core;

    public class FhirExtensionsBuilder : IFhirExtensionsBuilder
    {
        private readonly IStorageBuilder _fhirStoreBuilder;
        private readonly Uri _baseUri;
        private readonly IList<IFhirServiceExtension> _extensions;
        private readonly IIndexService _indexService;
        private readonly SparkSettings _sparkSettings;

        public FhirExtensionsBuilder(IStorageBuilder fhirStoreBuilder, Uri baseUri, IIndexService indexService, SparkSettings sparkSettings = null)
        {
            this._fhirStoreBuilder = fhirStoreBuilder;
            this._baseUri = baseUri;
            this._indexService = indexService;
            this._sparkSettings = sparkSettings;
            var extensionBuilders = new Func<IFhirServiceExtension>[]
           {
                GetSearch,
                GetHistory,
                GetCapabilityStatement,
                GetPaging,
                GetStorage
           };
            _extensions = extensionBuilders.Select(builder => builder()).Where(ext => ext != null).ToList();
        }

        protected virtual IFhirServiceExtension GetSearch()
        {
            var fhirStore = _fhirStoreBuilder.GetStore<IFhirIndex>();
            return fhirStore != null ? new SearchService(new Localhost(_baseUri), new FhirModel(), fhirStore, _indexService) : null;
        }

        protected virtual IFhirServiceExtension GetHistory()
        {
            var historyStore = _fhirStoreBuilder.GetStore<IHistoryStore>();

            return historyStore;
        }

        protected virtual IFhirServiceExtension GetCapabilityStatement()
        {
            return new CapabilityStatementService(new Localhost(_baseUri));
        }


        protected virtual IFhirServiceExtension GetPaging()
        {
            var fhirStore = _fhirStoreBuilder.GetStore<IFhirStore>();
            var snapshotStore = _fhirStoreBuilder.GetStore<ISnapshotStore>();
            var storeGenerator = _fhirStoreBuilder.GetStore<IGenerator>();
            return fhirStore != null
                ? new PagingService(snapshotStore, new SnapshotPaginationProvider(fhirStore, new Transfer(storeGenerator, new Localhost(_baseUri), _sparkSettings), new Localhost(_baseUri), new SnapshotPaginationCalculator()))
                : null;
        }

        protected virtual IFhirServiceExtension GetStorage()
        {
            var fhirStore = _fhirStoreBuilder.GetStore<IFhirStore>();
            var fhirGenerator = _fhirStoreBuilder.GetStore<IGenerator>();
            return fhirStore != null
                ? new ResourceStorageService(new Transfer(fhirGenerator, new Localhost(_baseUri), _sparkSettings), fhirStore)
                : null;
        }

        public IEnumerable<IFhirServiceExtension> GetExtensions()
        {
            return _extensions;
        }

        public IEnumerator<IFhirServiceExtension> GetEnumerator()
        {
            return _extensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}