namespace Spark.Engine.Web
{
    using System;
    using System.Collections.Generic;
    using Core;
    using FhirResponseFactory;
    using Formatters;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Search;
    using Service;
    using Service.FhirServiceExtensions;
    using Store.Interfaces;

    public static class IServiceCollectionExtensions
    {
        public static IMvcCoreBuilder AddFhir(this IServiceCollection services, SparkSettings settings, Action<MvcOptions> setupAction = null)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            services.AddFhirHttpSearchParameters();

            services.TryAddSingleton(settings);
            services.TryAddTransient<ElementIndexer>();


            services.TryAddTransient<IReferenceNormalizationService, ReferenceNormalizationService>();

            services.TryAddTransient<IIndexService, IndexService>();
            services.AddSingleton<ILocalhost>(new Localhost(settings.Endpoint));
            services.AddSingleton<IFhirModel>(new FhirModel(ModelInfo.SearchParameters));
            services.TryAddTransient((provider) => new FhirPropertyIndex(provider.GetRequiredService<IFhirModel>()));
            services.TryAddTransient<ITransfer, Transfer>();
            services.TryAddTransient<ConditionalHeaderFhirResponseInterceptor>();
            services.TryAddTransient((provider) => new IFhirResponseInterceptor[] { provider.GetRequiredService<ConditionalHeaderFhirResponseInterceptor>() });
            services.TryAddTransient<IFhirResponseInterceptorRunner, FhirResponseInterceptorRunner>();
            services.TryAddTransient<IFhirResponseFactory, Engine.FhirResponseFactory.FhirResponseFactory>();
            services.TryAddTransient<IIndexRebuildService, IndexRebuildService>();
            services.TryAddTransient<ISearchService, SearchService>();
            services.TryAddTransient<ISnapshotPaginationProvider, SnapshotPaginationProvider>();
            services.TryAddTransient<ISnapshotPaginationCalculator, SnapshotPaginationCalculator>();
            services.TryAddTransient<IServiceListener, SearchService>();   // searchListener
            services.TryAddTransient((provider) => new IServiceListener[] { provider.GetRequiredService<IServiceListener>() });
            services.TryAddTransient<SearchService>();                     // search
            services.TryAddTransient<TransactionService>();                // transaction
            //services.TryAddTransient<HistoryService>();                    // history
            services.TryAddTransient<PagingService>();                     // paging
            services.TryAddTransient<ResourceStorageService>();            // storage
            services.TryAddTransient<CapabilityStatementService>();        // conformance
            services.TryAddTransient<ICompositeServiceListener, ServiceListener>();
            services.TryAddTransient<JsonFhirInputFormatter>();
            services.TryAddTransient<JsonFhirOutputFormatter>();
            services.TryAddTransient<XmlFhirInputFormatter>();
            services.TryAddTransient<XmlFhirOutputFormatter>();

            services.AddTransient((provider) => new IFhirServiceExtension[]
            {
                provider.GetRequiredService<SearchService>(),
                provider.GetRequiredService<TransactionService>(),
                provider.GetRequiredService<IHistoryStore>(),
                provider.GetRequiredService<PagingService>(),
                provider.GetRequiredService<ResourceStorageService>(),
                provider.GetRequiredService<CapabilityStatementService>(),
            });

            services.TryAddSingleton((provider) => new FhirJsonParser(settings.ParserSettings));
            services.TryAddSingleton((provider) => new FhirXmlParser(settings.ParserSettings));
            services.TryAddSingleton((provder) => new FhirJsonSerializer(settings.SerializerSettings));
            services.TryAddSingleton((provder) => new FhirXmlSerializer(settings.SerializerSettings));

            services.TryAddSingleton<IFhirService, FhirService>();

            IMvcCoreBuilder builder = services.AddFhirFormatters(settings, setupAction);

            //services.RemoveAll<OutputFormatterSelector>();
            //services.TryAddSingleton<OutputFormatterSelector, FhirOutputFormatterSelector>();

            //services.RemoveAll<OutputFormatterSelector>();
            //services.TryAddSingleton<OutputFormatterSelector, FhirOutputFormatterSelector>();

            return builder;
        }

        public static IMvcCoreBuilder AddFhirFormatters(this IServiceCollection services, SparkSettings settings, Action<MvcOptions> setupAction = null)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            return services.AddMvcCore(options =>
            {
                options.InputFormatters.Add(new JsonFhirInputFormatter(new FhirJsonParser(settings.ParserSettings)));
                options.InputFormatters.Add(new XmlFhirInputFormatter(new FhirXmlParser(settings.ParserSettings)));
                options.InputFormatters.Add(new BinaryFhirInputFormatter());
                options.OutputFormatters.Add(new JsonFhirOutputFormatter(new FhirJsonSerializer(settings.SerializerSettings)));
                options.OutputFormatters.Add(new XmlFhirOutputFormatter(new FhirXmlSerializer(settings.SerializerSettings)));
                options.OutputFormatters.Add(new BinaryFhirOutputFormatter());

                options.RespectBrowserAcceptHeader = true;

                setupAction?.Invoke(options);
            });
        }

        public static void AddCustomSearchParameters(this IServiceCollection services, IEnumerable<ModelInfo.SearchParamDefinition> searchParameters)
        {
            // Add any user-supplied SearchParameters
            ModelInfo.SearchParameters.AddRange(searchParameters);
        }

        private static void AddFhirHttpSearchParameters(this IServiceCollection services)
        {
            ModelInfo.SearchParameters.AddRange(new[]
            {
                new ModelInfo.SearchParamDefinition { Resource = "Resource", Name = "_id", Type = SearchParamType.String, Path = new string[] { "Resource.id" } }
                , new ModelInfo.SearchParamDefinition { Resource = "Resource", Name = "_lastUpdated", Type = SearchParamType.Date, Path = new string[] { "Resource.meta.lastUpdated" } }
                , new ModelInfo.SearchParamDefinition { Resource = "Resource", Name = "_tag", Type = SearchParamType.Token, Path = new string[] { "Resource.meta.tag" } }
                , new ModelInfo.SearchParamDefinition { Resource = "Resource", Name = "_profile", Type = SearchParamType.Uri, Path = new string[] { "Resource.meta.profile" } }
                , new ModelInfo.SearchParamDefinition { Resource = "Resource", Name = "_security", Type = SearchParamType.Token, Path = new string[] { "Resource.meta.security" } }
            });
        }
    }
}
