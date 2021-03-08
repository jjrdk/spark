// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Interfaces;
    using Engine.Service;
    using Engine.Service.FhirServiceExtensions;
    using Hl7.Fhir.Model;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Models.Config;
    using Utilities;

    public class MaintenanceHub : Hub
    {
        private readonly ExamplesSettings _examplesSettings;
        private readonly IFhirIndex _fhirIndex;

        private readonly IAsyncFhirService _fhirService;
        private readonly IFhirStoreAdministration _fhirStoreAdministration;
        private readonly IHubContext<MaintenanceHub> _hubContext;
        private readonly IIndexRebuildService _indexRebuildService;
        private readonly ILogger<MaintenanceHub> _logger;

        private int _resourceCount;
        private List<Resource> _resources;

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
            var examplePath = Path.Combine(AppContext.BaseDirectory, _examplesSettings.FilePath);

            var data = FhirFileImport.ImportEmbeddedZip(examplePath).ToBundle();

            if (data.Entry != null && data.Entry.Count != 0)
            {
                list.AddRange(from entry in data.Entry where entry.Resource != null select entry.Resource);
            }

            return list;
        }

        private ImportProgressMessage Message(string message, int idx)
        {
            var msg = new ImportProgressMessage {Message = message, Progress = 10 + (idx + 1) * 90 / _resourceCount};
            return msg;
        }

        public async void ClearStore()
        {
            var notifier = new HubContextProgressNotifier(_hubContext, _logger);
            try
            {
                await notifier.SendProgressUpdate(0, "Clearing the database...").ConfigureAwait(false);
                await _fhirStoreAdministration.Clean().ConfigureAwait(false);
                await _fhirIndex.Clean().ConfigureAwait(false);
                await notifier.SendProgressUpdate(100, "Database cleared").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await notifier.SendProgressUpdate(100, "ERROR CLEARING :( " + e.InnerException).ConfigureAwait(false);
            }
        }

        public async void RebuildIndex()
        {
            var notifier = new HubContextProgressNotifier(_hubContext, _logger);
            try
            {
                await _indexRebuildService.RebuildIndexAsync(notifier).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to rebuild index");

                await notifier.SendProgressUpdate(100, "ERROR REBUILDING INDEX :( " + e.InnerException)
                    .ConfigureAwait(false);
            }
        }

        public async System.Threading.Tasks.Task LoadExamplesToStore()
        {
            var messages = new StringBuilder();
            var notifier = new HubContextProgressNotifier(_hubContext, _logger);
            try
            {
                await notifier.SendProgressUpdate(1, "Loading examples data...").ConfigureAwait(false);
                _resources = GetExampleData();

                var resarray = _resources.ToArray();
                _resourceCount = resarray.Length;

                for (var x = 0; x <= _resourceCount - 1; x++)
                {
                    var res = resarray[x];
                    // Sending message:
                    var msg = Message("Importing " + res.TypeName + " " + res.Id + "...", x);
                    await notifier.SendProgressUpdate(msg.Progress, msg.Message).ConfigureAwait(false);

                    try
                    {
                        var key = res.ExtractKey();

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
                        // Sending message:
                        var msgError = Message("ERROR Importing " + res.TypeName + " " + res.Id + "... ", x);
                        await Clients.All.SendAsync("Error", msg).ConfigureAwait(false);
                        messages.AppendLine(msgError.Message + ": " + e.Message);
                    }
                }

                await notifier.SendProgressUpdate(100, messages.ToString()).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await notifier.Progress("Error: " + e.Message).ConfigureAwait(false);
            }
        }
    }
}
