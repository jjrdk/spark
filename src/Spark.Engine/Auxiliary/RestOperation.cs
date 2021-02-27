// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Auxiliary
{
    using Hl7.Fhir.Rest;

    public static class RestOperation
    {
        // API: this constant can be derived from TransactionBuilder.
        // But the History keyword has a bigger scope than just TransactionBuilder.
        // proposal: move HISTORY and other URL/operation constants to Hl7.Fhir.Rest.Operation or something.
        public static string History = TransactionBuilder.HISTORY;
    }
}