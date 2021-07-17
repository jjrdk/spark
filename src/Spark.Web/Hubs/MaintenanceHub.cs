namespace Spark.Web.Hubs
{
    using Hl7.Fhir.Model;
    using Spark.Engine.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Spark.Engine.Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Spark.Web.Models.Config;
    using Spark.Web.Utilities;
    using System.IO;
    using Engine.Extensions;
    using Microsoft.Extensions.Logging;
    using Spark.Engine.Service;
    using Spark.Engine.Service.FhirServiceExtensions;

    //[Authorize(Policy = "RequireAdministratorRole")]
    public class MaintenanceHub : Hub
    {
        private List<Resource> _resources = null;

        private readonly IAsyncFhirService _fhirService;
        private readonly IFhirStoreAdministration _fhirStoreAdministration;
        private readonly IFhirIndex _fhirIndex;
        private readonly ExamplesSettings _examplesSettings;
        private readonly IIndexRebuildService _indexRebuildService;
        private readonly ILogger<MaintenanceHub> _logger;
        private readonly IHubContext<MaintenanceHub> _hubContext;

        private int _resourceCount;

        public MaintenanceHub(
            IAsyncFhirService fhirService,
            IFhirStoreAdministration fhirStoreAdministration,
            IFhirIndex fhirIndex,
            ExamplesSettings examplesSettings,
            IIndexRebuildService indexRebuildService,
            ILogger<MaintenanceHub> logger,
            IHubContext<MaintenanceHub> hubContext)
        {
            _fhirService = fhirService;
            _fhirStoreAdministration = fhirStoreAdministration;
            _fhirIndex = fhirIndex;
            _examplesSettings = examplesSettings;
            _indexRebuildService = indexRebuildService;
            _logger = logger;
            _hubContext = hubContext;
        }

        public List<Resource> GetExampleData()
        {
            var list = new List<Resource>();
            string examplePath = Path.Combine(AppContext.BaseDirectory, _examplesSettings.FilePath);

            Bundle data;
            data = FhirFileImport.ImportEmbeddedZip(examplePath).ToBundle();

            if (data.Entry != null && data.Entry.Count != 0)
            {
                foreach (var entry in data.Entry)
                {
                    if (entry.Resource != null)
                    {
                        list.Add((Resource)entry.Resource);
                    }
                }
            }
            return list;
        }

        public async void ClearStore()
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Starting clearing database...").ConfigureAwait(false);
                await _fhirStoreAdministration.Clean().ConfigureAwait(false);

                await _hubContext.Clients.All.SendAsync("UpdateProgress", "... and cleaning indexes...").ConfigureAwait(false);
                await _fhirIndex.Clean().ConfigureAwait(false);
                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Database cleared").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to clear store.");
                await _hubContext.Clients.All.SendAsync("UpdateProgress", $"ERROR CLEARING :(").ConfigureAwait(false);
            }

        }

        public async void RebuildIndex()
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Rebuilding index...").ConfigureAwait(false);
                await _indexRebuildService.RebuildIndexAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to rebuild index");

                await _hubContext.Clients.All.SendAsync("UpdateProgress", "ERROR REBUILDING INDEX :( ")
                    .ConfigureAwait(false);
            }
            await _hubContext.Clients.All.SendAsync("UpdateProgress", "Index rebuilt!").ConfigureAwait(false);
        }

        public async void LoadExamplesToStore()
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Loading examples").ConfigureAwait(false);
                _resources = GetExampleData();

                var resarray = _resources.ToArray();
                _resourceCount = resarray.Length;

                for (int x = 0; x <= _resourceCount - 1; x++)
                {
                    var res = resarray[x];
                    var msg = $"Importing {res.TypeName}, id {res.Id} ...";
                    await _hubContext.Clients.All.SendAsync("UpdateProgress", msg).ConfigureAwait(false);

                    try
                    {
                        Key key = res.ExtractKey();

                        if (!string.IsNullOrEmpty(res.Id))
                        {
                            await _fhirService.Put(key, res).ConfigureAwait(false);
                        }
                        else
                        {
                            await _fhirService.Create(key, res).ConfigureAwait(false);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed when loading example.");
                        var msgError = $"ERROR Importing {res.TypeName}, id {res.Id}...";
                        await _hubContext.Clients.All.SendAsync("UpdateProgress", msgError).ConfigureAwait(false);
                    }
                }

                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Finished loading examples").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load examples.");
                await _hubContext.Clients.All.SendAsync("UpdateProgress", "Error: " + e.Message).ConfigureAwait(false);
            }
        }
    }
}
