// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Service.FhirServiceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Hl7.Fhir.Model;

    public interface ITransactionService
    {
        Task<FhirResponse> HandleTransaction(
            ResourceManipulationOperation operation,
            IInteractionHandler interactionHandler);

        Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(
            Bundle bundle,
            IInteractionHandler interactionHandler);

        Task<IList<Tuple<Entry, FhirResponse>>> HandleTransaction(
            IList<Entry> interactions,
            IInteractionHandler interactionHandler);
    }
}