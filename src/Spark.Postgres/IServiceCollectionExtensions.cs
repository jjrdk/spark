namespace Spark.Postgres
{
    using System;
    using Core;
    using Engine;
    using Engine.Interfaces;
    using Engine.Store.Interfaces;
    using Marten;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;

    public static class ServiceCollectionExtensions
    {
        public static void AddPostgresFhirStore(this IServiceCollection services, StoreSettings settings)
        {
            var store = DocumentStore.For(
                o =>
                {
                    o.Connection(settings.ConnectionString);
                    o.PLV8Enabled = false;
                    o.Schema.Include<FhirRegistry>();
                });
            services.AddSingleton<IDocumentStore>(store);
            services.TryAddSingleton(settings);
            services.AddSingleton<IGenerator>(new GuidGenerator());
            services.AddTransient<Func<IDocumentSession>>(
             sp => () => sp.GetRequiredService<IDocumentStore>().OpenSession());
            services.TryAddTransient<IFhirStore>(
             provider => new MartenFhirStore(provider.GetRequiredService<Func<IDocumentSession>>()));
            services.TryAddTransient<IFhirStorePagedReader>(
             provider => new MartenFhirStorePagedReader(provider.GetRequiredService<Func<IDocumentSession>>()));
            services.TryAddTransient<IHistoryStore>(
             provider => new MartenHistoryStore(provider.GetRequiredService<Func<IDocumentSession>>()));
            services.TryAddTransient<ISnapshotStore>(
             provider => new MartenSnapshotStore(
                 provider.GetRequiredService<Func<IDocumentSession>>(),
                 provider.GetRequiredService<ILogger<MartenSnapshotStore>>()));
            services.TryAddTransient<IFhirStoreAdministration>(
             provider => new MartenFhirStoreAdministration(provider.GetRequiredService<Func<IDocumentSession>>()));
            services.TryAddTransient<IIndexStore>(
             sp => new MartenFhirIndex(
                 sp.GetRequiredService<ILogger<MartenFhirIndex>>(),
                 sp.GetRequiredService<Func<IDocumentSession>>()));
            services.TryAddTransient<IFhirIndex>(
             sp => new MartenFhirIndex(
                 sp.GetRequiredService<ILogger<MartenFhirIndex>>(),
                 sp.GetRequiredService<Func<IDocumentSession>>()));
        }
    }
}
