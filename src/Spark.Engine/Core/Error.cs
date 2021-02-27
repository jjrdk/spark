// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Core
{
    using System.Collections.Generic;
    using System.Net;
    using Hl7.Fhir.Model;

    public static class Error
    {
        public static SparkException Create(HttpStatusCode code, string message, params object[] values) =>
            new SparkException(code, message, values);

        public static SparkException BadRequest(string message, params object[] values) =>
            new SparkException(HttpStatusCode.BadRequest, message, values);

        public static SparkException NotFound(string message, params object[] values) =>
            new SparkException(HttpStatusCode.NotFound, message, values);

        public static SparkException NotFound(IKey key) =>
            key.VersionId == null
                ? NotFound("No {0} resource with id {1} was found.", key.TypeName, key.ResourceId)
                : NotFound(
                    "There is no {0} resource with id {1}, or there is no version {2}",
                    key.TypeName,
                    key.ResourceId,
                    key.VersionId);

        public static SparkException NotAllowed(string message) =>
            new SparkException(HttpStatusCode.Forbidden, message);

        public static SparkException Internal(string message, params object[] values) =>
            new SparkException(HttpStatusCode.InternalServerError, message, values);

        public static SparkException NotSupported(string message, params object[] values) =>
            new SparkException(HttpStatusCode.NotImplemented, message, values);

        private static OperationOutcome.IssueComponent
            CreateValidationResult(string details, IEnumerable<string> location) =>
            new OperationOutcome.IssueComponent
            {
                Severity = OperationOutcome.IssueSeverity.Error,
                Code = OperationOutcome.IssueType.Invalid,
                Details = new CodeableConcept("http://hl7.org/fhir/issue-type", "invalid"),
                Diagnostics = details,
                Location = location
            };
    }
}