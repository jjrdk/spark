﻿namespace Spark.Engine.Auxiliary
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
