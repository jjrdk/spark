using Hl7.Fhir.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spark.Engine;
using Spark.Engine.Interfaces;
using Spark.Engine.Store.Interfaces;
using Spark.Mongo.Search.Common;
using Spark.Mongo.Search.Indexer;
using Spark.Mongo.Store;
using Spark.Mongo.Store.Extensions;

namespace Spark.Mongo.Extensions
{
    using Core;
    using Search.Infrastructure;
    using Search.Searcher;
    using Spark.Search.Mongo;
    using Spark.Store.Mongo;

    public static class ServiceCollectionExtensions
    {
        public static void AddMongoFhirStore(this IServiceCollection services, StoreSettings settings)
        {
            services.TryAddSingleton(settings);
            services.TryAddTransient<IGenerator>(provider => new MongoIdGenerator(settings.ConnectionString));
            services.TryAddTransient<IFhirStore>(provider => new MongoFhirStore(settings.ConnectionString));
            services.TryAddTransient<IFhirStorePagedReader>(provider => new MongoFhirStorePagedReader(settings.ConnectionString));
            services.TryAddTransient<IHistoryStore>(provider => new HistoryStore(settings.ConnectionString));
            services.TryAddTransient<ISnapshotStore>(provider => new MongoSnapshotStore(settings.ConnectionString));
            services.TryAddTransient<IFhirStoreAdministration>(provider => new MongoStoreAdministration(settings.ConnectionString));
            services.TryAddTransient<MongoIndexMapper>();
            services.TryAddTransient<IIndexStore>(provider => new MongoIndexStore(settings.ConnectionString, provider.GetRequiredService<MongoIndexMapper>()));
            services.TryAddTransient(provider => new MongoIndexStore(settings.ConnectionString, provider.GetRequiredService<MongoIndexMapper>()));
            services.TryAddTransient(provider => DefinitionsFactory.Generate(ModelInfo.SearchParameters));
            services.TryAddTransient<MongoSearcher>();
            services.TryAddTransient<IFhirIndex, MongoFhirIndex>();
        }
    }
}
