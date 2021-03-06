﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;

    public interface IAsyncFhirService
    {
        Task<FhirResponse> AddMeta(IKey key, Parameters parameters);
        Task<FhirResponse> ConditionalCreate(IKey key, Resource resource, IEnumerable<Tuple<string, string>> parameters);
        Task<FhirResponse> ConditionalCreate(IKey key, Resource resource, SearchParams parameters);
        Task<FhirResponse> ConditionalDelete(IKey key, IEnumerable<Tuple<string, string>> parameters);
        Task<FhirResponse> ConditionalUpdate(IKey key, Resource resource, SearchParams parameters);
        Task<FhirResponse> CapabilityStatement(string sparkVersion);
        Task<FhirResponse> Create(IKey key, Resource resource);
        Task<FhirResponse> Delete(IKey key);
        Task<FhirResponse> Delete(Entry entry);
        Task<FhirResponse> GetPage(string snapshotKey, int index);
        Task<FhirResponse> History(HistoryParameters parameters);
        Task<FhirResponse> History(string type, HistoryParameters parameters);
        Task<FhirResponse> History(IKey key, HistoryParameters parameters);
        Task<FhirResponse> Mailbox(Bundle bundle, Binary body);
        Task<FhirResponse> Put(IKey key, Resource resource);
        Task<FhirResponse> Put(Entry entry);
        Task<FhirResponse> Read(IKey key, ConditionalHeaderParameters parameters = null);
        Task<FhirResponse> ReadMeta(IKey key);
        Task<FhirResponse> Search(string type, SearchParams searchCommand, int pageIndex = 0);
        Task<FhirResponse> Transaction(params Entry[] interactions);
        Task<FhirResponse> Transaction(Bundle bundle);
        Task<FhirResponse> Update(IKey key, Resource resource);
        Task<FhirResponse> Patch(IKey key, Parameters patch);

        Task<FhirResponse> ValidateOperation(IKey key, Resource resource);
        Task<FhirResponse> VersionRead(IKey key);
        Task<FhirResponse> VersionSpecificUpdate(IKey versionedKey, Resource resource);
        Task<FhirResponse> Everything(IKey key);
        Task<FhirResponse> Document(IKey key);
    }
}