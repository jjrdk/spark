namespace Spark.Engine.Service.FhirServiceExtensions
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
        private readonly IStorageBuilder fhirStoreBuilder;
        private readonly Uri baseUri;
        private readonly IList<IFhirServiceExtension> extensions;
        private readonly IIndexService indexService;
        private readonly SparkSettings sparkSettings;

        public FhirExtensionsBuilder(IStorageBuilder fhirStoreBuilder, Uri baseUri, IIndexService indexService, SparkSettings sparkSettings = null)
        {
            this.fhirStoreBuilder = fhirStoreBuilder;
            this.baseUri = baseUri;
            this.indexService = indexService;
            this.sparkSettings = sparkSettings;
            var extensionBuilders = new Func<IFhirServiceExtension>[]
           {
                GetSearch,
                GetHistory,
                GetCapabilityStatement,
                GetPaging,
                GetStorage
           };
            extensions = extensionBuilders.Select(builder => builder()).Where(ext => ext != null).ToList();
        }

        protected virtual IFhirServiceExtension GetSearch()
        {
            var fhirStore = fhirStoreBuilder.GetStore<IFhirIndex>();
            if (fhirStore != null)
                return new SearchService(new Localhost(baseUri), new FhirModel(), fhirStore, indexService);
            return null;
        }

        protected virtual IFhirServiceExtension GetHistory()
        {
            var historyStore = fhirStoreBuilder.GetStore<IHistoryStore>();

            return historyStore;
        }

        protected virtual IFhirServiceExtension GetCapabilityStatement()
        {
            return new CapabilityStatementService(new Localhost(baseUri));
        }


        protected virtual IFhirServiceExtension GetPaging()
        {
            var fhirStore = fhirStoreBuilder.GetStore<IFhirStore>();
            var snapshotStore = fhirStoreBuilder.GetStore<ISnapshotStore>();
            var storeGenerator = fhirStoreBuilder.GetStore<IGenerator>();
            if (fhirStore != null)
                return new PagingService(snapshotStore, new SnapshotPaginationProvider(fhirStore, new Transfer(storeGenerator, new Localhost(baseUri), sparkSettings), new Localhost(baseUri), new SnapshotPaginationCalculator()));
            return null;
        }

        protected virtual IFhirServiceExtension GetStorage()
        {
            var fhirStore = fhirStoreBuilder.GetStore<IFhirStore>();
            var fhirGenerator = fhirStoreBuilder.GetStore<IGenerator>();
            if (fhirStore != null)
                return new ResourceStorageService(new Transfer(fhirGenerator, new Localhost(baseUri), sparkSettings), fhirStore);
            return null;
        }

        public IEnumerable<IFhirServiceExtension> GetExtensions()
        {
            return extensions;
        }

        public IEnumerator<IFhirServiceExtension> GetEnumerator()
        {
            return extensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}