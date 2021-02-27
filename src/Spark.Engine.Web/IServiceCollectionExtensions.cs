// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

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

    public static class ServiceCollectionExtensions
    {
        public static IMvcCoreBuilder AddFhir(
            this IServiceCollection services,
            SparkSettings settings,
            Action<MvcOptions> setupAction = null,
            Func<IServiceProvider, IInteractionHandler> interactionHandler = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            services.AddFhirHttpSearchParameters();

            services.TryAddSingleton(settings);
            services.TryAddTransient<ElementIndexer>();


            services.TryAddTransient<IReferenceNormalizationService, ReferenceNormalizationService>();

            services.TryAddTransient<IIndexService, IndexService>();
            services.AddSingleton<ILocalhost>(new Localhost(settings.Endpoint));
            services.AddSingleton<IFhirModel>(new FhirModel(ModelInfo.SearchParameters));
            services.TryAddTransient(provider => new FhirPropertyIndex(provider.GetRequiredService<IFhirModel>()));
            services.TryAddTransient<ITransfer, Transfer>();
            services.TryAddTransient<ConditionalHeaderFhirResponseInterceptor>();
            services.TryAddTransient(
                provider => new IFhirResponseInterceptor[]
                {
                    provider.GetRequiredService<ConditionalHeaderFhirResponseInterceptor>()
                });
            services.TryAddTransient<IFhirResponseInterceptorRunner, FhirResponseInterceptorRunner>();
            services.TryAddTransient<IFhirResponseFactory, FhirResponseFactory>();
            services.TryAddTransient<IIndexRebuildService, IndexRebuildService>();
            services.TryAddTransient<ISearchService, SearchService>();
            services.TryAddTransient<ISnapshotPaginationProvider, SnapshotPaginationProvider>();
            services.TryAddTransient<ISnapshotPaginationCalculator, SnapshotPaginationCalculator>();
            services.TryAddTransient<IServiceListener, SearchService>(); // searchListener
            services.TryAddTransient(provider => new[] { provider.GetRequiredService<IServiceListener>() });
            services.TryAddTransient<SearchService>(); // search
            services.TryAddTransient<TransactionService>(); // transaction
            //services.TryAddTransient<HistoryService>();                    // history
            services.TryAddTransient<PagingService>(); // paging
            services.TryAddTransient<ResourceStorageService>(); // storage
            services.TryAddTransient<CapabilityStatementService>(); // conformance
            services.TryAddTransient<ICompositeServiceListener, ServiceListener>();
            services.TryAddTransient<JsonFhirInputFormatter>();
            services.TryAddTransient<JsonFhirOutputFormatter>();
            services.TryAddTransient<XmlFhirInputFormatter>();
            services.TryAddTransient<XmlFhirOutputFormatter>();
            if (interactionHandler == null)
            {
                services.TryAddTransient<IInteractionHandler, DefaultInteractionHandler>();
            }
            else
            {
                services.TryAddTransient(interactionHandler);
            }
            services.AddTransient(
                provider => new IFhirServiceExtension[]
                {
                    provider.GetRequiredService<SearchService>(),
                    provider.GetRequiredService<TransactionService>(),
                    provider.GetRequiredService<IHistoryStore>(),
                    provider.GetRequiredService<PagingService>(),
                    provider.GetRequiredService<ResourceStorageService>(),
                    provider.GetRequiredService<CapabilityStatementService>()
                });

            services.TryAddSingleton(_ => new FhirJsonParser(settings.ParserSettings));
            services.TryAddSingleton(_ => new FhirXmlParser(settings.ParserSettings));
            services.TryAddSingleton(_ => new FhirJsonSerializer(settings.SerializerSettings));
            services.TryAddSingleton(_ => new FhirXmlSerializer(settings.SerializerSettings));

            services.TryAddSingleton<IAsyncFhirService, AsyncFhirService>();

            var builder = services.AddFhirFormatters(settings, setupAction);

            return builder;
        }

        public static IMvcCoreBuilder AddFhirFormatters(
            this IServiceCollection services,
            SparkSettings settings,
            Action<MvcOptions> setupAction = null)
        {
            return settings == null
                ? throw new ArgumentNullException(nameof(settings))
                : services.AddMvcCore(
                    options =>
                    {
                        options.InputFormatters.Add(
                            new JsonFhirInputFormatter(new FhirJsonParser(settings.ParserSettings)));
                        options.InputFormatters.Add(
                            new XmlFhirInputFormatter(new FhirXmlParser(settings.ParserSettings)));
                        options.InputFormatters.Add(new BinaryFhirInputFormatter());
                        options.OutputFormatters.Add(
                            new JsonFhirOutputFormatter(new FhirJsonSerializer(settings.SerializerSettings)));
                        options.OutputFormatters.Add(
                            new XmlFhirOutputFormatter(new FhirXmlSerializer(settings.SerializerSettings)));
                        options.OutputFormatters.Add(new BinaryFhirOutputFormatter());

                        options.RespectBrowserAcceptHeader = true;

                        setupAction?.Invoke(options);
                    });
        }

        public static void AddCustomSearchParameters(
            this IServiceCollection _,
            IEnumerable<ModelInfo.SearchParamDefinition> searchParameters)
        {
            // Add any user-supplied SearchParameters
            ModelInfo.SearchParameters.AddRange(searchParameters);
        }

        private static void AddFhirHttpSearchParameters(this IServiceCollection _)
        {
            ModelInfo.SearchParameters.AddRange(
                new[]
                {
                    new ModelInfo.SearchParamDefinition
                    {
                        Resource = "Resource",
                        Name = "_id",
                        Type = SearchParamType.String,
                        Path = new[] {"Resource.id"}
                    },
                    new ModelInfo.SearchParamDefinition
                    {
                        Resource = "Resource",
                        Name = "_lastUpdated",
                        Type = SearchParamType.Date,
                        Path = new[] {"Resource.meta.lastUpdated"}
                    },
                    new ModelInfo.SearchParamDefinition
                    {
                        Resource = "Resource",
                        Name = "_tag",
                        Type = SearchParamType.Token,
                        Path = new[] {"Resource.meta.tag"}
                    },
                    new ModelInfo.SearchParamDefinition
                    {
                        Resource = "Resource",
                        Name = "_profile",
                        Type = SearchParamType.Uri,
                        Path = new[] {"Resource.meta.profile"}
                    },
                    new ModelInfo.SearchParamDefinition
                    {
                        Resource = "Resource",
                        Name = "_security",
                        Type = SearchParamType.Token,
                        Path = new[] {"Resource.meta.security"}
                    }
                });
        }
    }
}