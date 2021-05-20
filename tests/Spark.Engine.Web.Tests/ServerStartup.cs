namespace Spark.Engine.Web.Tests
{
    using System;
    using Engine.Service.FhirServiceExtensions;
    using Hl7.Fhir.Serialization;
    using Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using Store.Interfaces;
    using Xunit.Abstractions;

    public class ServerStartup
    {
        private readonly ITestOutputHelper _outputHelper;

        public ServerStartup(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(l => l.AddXunit(_outputHelper));
            services.AddFhir(
                new SparkSettings
                {
                    UseAsynchronousIO = false,
                    Endpoint = new Uri("https://localhost:6001/fhir"),
                    FhirRelease = "R4",
                    ParserSettings = ParserSettings.CreateDefault(),
                    SerializerSettings = SerializerSettings.CreateDefault()
                });
            services.AddSingleton<IFhirIndex, InMemoryFhirIndex>();
            services.AddSingleton<ISnapshotStore, InMemorySnapshotStore>();
            services.AddSingleton<IHistoryStore, InMemoryHistoryStore>();
            services.AddSingleton<IGenerator, GuidGenerator>();
            services.AddSingleton<IFhirStore, InMemoryFhirStore>();
            services.AddSingleton<IIndexStore, NoopIndexStore>();
            services.AddSingleton<IPatchService, PatchService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting()
                //.UseAuthentication().UseAuthorization()
                .UseEndpoints(e => e.MapControllers());
        }
    }
}