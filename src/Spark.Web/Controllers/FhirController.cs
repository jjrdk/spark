// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using Engine;
    using Engine.Core;
    using Engine.Extensions;
    using Engine.Service;
    using Engine.Utility;
    using Engine.Web.Extensions;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("fhir")]
    [ApiController]
    [EnableCors]
    public class FhirController : ControllerBase
    {
        private readonly IAsyncFhirService _fhirService;

        public FhirController(IAsyncFhirService fhirService) =>
            _fhirService = fhirService ?? throw new ArgumentNullException(nameof(fhirService));

        [HttpGet("{type}/{id}")]
        public async Task<ActionResult<FhirResponse>> Read(string type, string id)
        {
            var parameters = new ConditionalHeaderParameters(Request.IfNoneMatch(), Request.IfModifiedSince());
            var key = Key.Create(type, id);
            var response = await _fhirService.Read(key, parameters).ConfigureAwait(false);
            return new ActionResult<FhirResponse>(response);
        }

        [HttpGet("{type}/{id}/_history/{vid}")]
        public async Task<FhirResponse> VRead(string type, string id, string vid)
        {
            var key = Key.Create(type, id, vid);
            return await _fhirService.VersionRead(key).ConfigureAwait(false);
        }

        [HttpPut("{type}/{id?}")]
        public async Task<ActionResult<FhirResponse>> Update(string type, Resource resource, string id = null)
        {
            var versionId = Request.GetTypedHeaders().IfMatch?.FirstOrDefault()?.Tag.Buffer;
            var key = Key.Create(type, id, versionId);
            if (key.HasResourceId())
            {
                Request.TransferResourceIdIfRawBinary(resource, id);

                return new ActionResult<FhirResponse>(await _fhirService.Update(key, resource).ConfigureAwait(false));
            }

            return new ActionResult<FhirResponse>(
                await _fhirService.ConditionalUpdate(
                        key,
                        resource,
                        SearchParams.FromUriParamList(Request.TupledParameters()))
                    .ConfigureAwait(false));
        }

        [HttpPatch("{type}/{id}")]
        public async Task<ActionResult<FhirResponse>> Patch(string type, string id, Parameters patch)
        {
            var key = Key.Create(type, id);
            var response = await _fhirService.Patch(key, patch).ConfigureAwait(false);
            return new ActionResult<FhirResponse>(response);
        }

        [HttpPost("{type}")]
        public async Task<FhirResponse> Create(string type, Resource resource)
        {
            var key = Key.Create(type, resource?.Id);

            if (Request.Headers.ContainsKey(FhirHttpHeaders.IF_NONE_EXIST))
            {
                var searchQueryString = HttpUtility.ParseQueryString(Request.GetTypedHeaders().IfNoneExist());
                var searchValues = searchQueryString.Keys.Cast<string>()
                    .Select(k => new Tuple<string, string>(k, searchQueryString[k]));

                return await _fhirService.ConditionalCreate(key, resource, SearchParams.FromUriParamList(searchValues))
                    .ConfigureAwait(false);
            }

            return await _fhirService.Create(key, resource).ConfigureAwait(false);
        }

        [HttpDelete("{type}/{id}")]
        public async Task<FhirResponse> Delete(string type, string id)
        {
            var key = Key.Create(type, id);
            var response = await _fhirService.Delete(key).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("{type}")]
        public async Task<FhirResponse> ConditionalDelete(string type)
        {
            var key = Key.Create(type);
            return await _fhirService.ConditionalDelete(key, Request.TupledParameters()).ConfigureAwait(false);
        }

        [HttpGet("{type}/{id}/_history")]
        public async Task<FhirResponse> History(string type, string id)
        {
            var key = Key.Create(type, id);
            var parameters = Request.ToHistoryParameters();
            return await _fhirService.History(key, parameters).ConfigureAwait(false);
        }

        // ============= Validate

        [HttpPost("{type}/{id}/$validate")]
        public async Task<FhirResponse> Validate(string type, string id, Resource resource)
        {
            var key = Key.Create(type, id);
            return await _fhirService.ValidateOperation(key, resource).ConfigureAwait(false);
        }

        [HttpPost("{type}/$validate")]
        public async Task<FhirResponse> Validate(string type, Resource resource)
        {
            var key = Key.Create(type);
            return await _fhirService.ValidateOperation(key, resource).ConfigureAwait(false);
        }

        // ============= Type Level Interactions

        [HttpGet("{type}")]
        public async Task<FhirResponse> Search(string type)
        {
            var start = Request.GetParameter(FhirParameter.SNAPSHOT_INDEX).ParseIntParameter() ?? 0;
            var searchparams = Request.GetSearchParams();
            //int pagesize = Request.GetIntParameter(FhirParameter.COUNT) ?? Const.DEFAULT_PAGE_SIZE;
            //string sortby = Request.GetParameter(FhirParameter.SORT);

            return await _fhirService.Search(type, searchparams, start).ConfigureAwait(false);
        }

        [HttpPost("{type}/_search")]
        public async Task<FhirResponse> SearchWithOperator(string type)
        {
            // TODO: start index should be retrieved from the body.
            var start = Request.GetParameter(FhirParameter.SNAPSHOT_INDEX).ParseIntParameter() ?? 0;
            var searchparams = Request.GetSearchParamsFromBody();

            return await _fhirService.Search(type, searchparams, start).ConfigureAwait(false);
        }

        [HttpGet("{type}/_history")]
        public async Task<FhirResponse> History(string type)
        {
            var parameters = Request.ToHistoryParameters();
            return await _fhirService.History(type, parameters).ConfigureAwait(false);
        }

        // ============= Whole System Interactions

        [HttpGet]
        [Route("metadata")]
        public async Task<FhirResponse> Metadata() =>
            await _fhirService.CapabilityStatement(SparkSettings.Version).ConfigureAwait(false);

        [HttpOptions]
        [Route("")]
        public async Task<FhirResponse> Options() =>
            await _fhirService.CapabilityStatement(SparkSettings.Version).ConfigureAwait(false);

        [HttpPost]
        [Route("")]
        public async Task<FhirResponse> Transaction(Bundle bundle) =>
            await _fhirService.Transaction(bundle).ConfigureAwait(false);

        //[HttpPost, Route("Mailbox")]
        //public FhirResponse Mailbox(Bundle document)
        //{
        //    Binary b = Request.GetBody();
        //    return service.Mailbox(document, b);
        //}

        [HttpGet]
        [Route("_history")]
        public async Task<FhirResponse> History()
        {
            var parameters = Request.ToHistoryParameters();
            return await _fhirService.History(parameters).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("_snapshot")]
        public async Task<FhirResponse> Snapshot()
        {
            var snapshot = Request.GetParameter(FhirParameter.SNAPSHOT_ID);
            var start = Request.GetParameter(FhirParameter.SNAPSHOT_INDEX).ParseIntParameter() ?? 0;
            return await _fhirService.GetPage(snapshot, start).ConfigureAwait(false);
        }

        // Operations

        [HttpPost]
        [Route("${operation}")]
        public FhirResponse ServerOperation(string operation)
        {
            return operation.ToLower() switch
            {
                "error" => throw new Exception("This error is for testing purposes"),
                _ => Respond.WithError(HttpStatusCode.NotFound, "Unknown operation")
            };
        }

        [HttpPost]
        [Route("{type}/{id}/${operation}")]
        public async Task<FhirResponse> InstanceOperation(
            string type,
            string id,
            string operation,
            Parameters parameters)
        {
            var key = Key.Create(type, id);
            return operation.ToLower() switch
            {
                "meta" => await _fhirService.ReadMeta(key).ConfigureAwait(false),
                "meta-add" => await _fhirService.AddMeta(key, parameters).ConfigureAwait(false),
                "meta-delete" => Respond.WithError(HttpStatusCode.NotFound, "Unknown operation"),
                _ => Respond.WithError(HttpStatusCode.NotFound, "Unknown operation")
            };
        }

        [HttpPost]
        [HttpGet]
        [Route("{type}/{id}/$everything")]
        public async Task<FhirResponse> Everything(string type, string id)
        {
            var key = Key.Create(type, id);
            return await _fhirService.Everything(key).ConfigureAwait(false);
        }

        [HttpPost]
        [HttpGet]
        [Route("{type}/$everything")]
        public async Task<FhirResponse> Everything(string type)
        {
            var key = Key.Create(type);
            return await _fhirService.Everything(key).ConfigureAwait(false);
        }

        [HttpPost]
        [HttpGet]
        [Route("Composition/{id}/$document")]
        public async Task<FhirResponse> Document(string id)
        {
            var key = Key.Create("Composition", id);
            return await _fhirService.Document(key).ConfigureAwait(false);
        }
    }
}