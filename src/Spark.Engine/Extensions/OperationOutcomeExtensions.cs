/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */
namespace Spark.Engine.Extensions
{
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Spark.Engine.Core;
    using System.Diagnostics;


    public static class OperationOutcomeExtensions
    {
        //internal static Func<string, string> pascalToCamelCase = (pascalCase) => $"{char.ToLower(pascalCase[0])}{pascalCase[1..]}";

        //public static OperationOutcome AddValidationProblems(this OperationOutcome outcome, Type resourceType, HttpStatusCode code, ValidationProblemDetails validationProblems)
        //{
        //    if (resourceType == null) throw new ArgumentNullException(nameof(resourceType));
        //    if (validationProblems == null) throw new ArgumentNullException(nameof(ValidationProblemDetails));

        //    OperationOutcome.IssueSeverity severity = IssueSeverityOf(code);
        //    foreach (var error in validationProblems.Errors)
        //    {
        //        var expression = FhirPathUtil.ResolveToFhirPathExpression(resourceType, error.Key);
        //        outcome.Issue.Add(new OperationOutcome.IssueComponent
        //        {
        //            Severity = severity,
        //            Code = OperationOutcome.IssueType.Required,
        //            Diagnostics = error.Value.FirstOrDefault(),
        //            Expression = new[] { expression },
        //            Location = new[] { FhirPathUtil.ConvertToXPathExpression(expression) }
        //        });
        //    }

        //    return outcome;
        //}

        internal static OperationOutcome.IssueSeverity IssueSeverityOf(HttpStatusCode code)
        {
            var range = ((int)code / 100);
            return range switch
            {
                1 => OperationOutcome.IssueSeverity.Information,
                2 => OperationOutcome.IssueSeverity.Information,
                3 => OperationOutcome.IssueSeverity.Warning,
                4 => OperationOutcome.IssueSeverity.Error,
                5 => OperationOutcome.IssueSeverity.Fatal,
                _ => OperationOutcome.IssueSeverity.Information
            };
        }

        private static void SetContentHeaders(HttpResponseMessage response, ResourceFormat format)
        {
            response.Content.Headers.ContentType = FhirMediaType.GetMediaTypeHeaderValue(typeof(Resource), format);
        }

        public static OperationOutcome Init(this OperationOutcome outcome)
        {
            if (outcome.Issue == null)
            {
                outcome.Issue = new List<OperationOutcome.IssueComponent>();
            }
            return outcome;
        }

        public static OperationOutcome AddError(this OperationOutcome outcome, Exception exception)
        {
            string message;

            if (exception is SparkException)
            {
                message = exception.Message;
            }
            else
            {
                message = $"{exception.GetType().Name}: {exception.Message}";
            }

            outcome.AddError(message);

            // Don't add a stacktrace if this is an acceptable logical-level error
            if (Debugger.IsAttached && !(exception is SparkException))
            {
                var stackTrace = new OperationOutcome.IssueComponent
                {
                    Severity = OperationOutcome.IssueSeverity.Information,
                    Diagnostics = exception.StackTrace
                };
                outcome.Issue.Add(stackTrace);
            }

            return outcome;
        }

        public static OperationOutcome AddAllInnerErrors(this OperationOutcome outcome, Exception exception)
        {
            AddError(outcome, exception);
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                AddError(outcome, exception);
            }

            return outcome;
        }

        public static OperationOutcome AddError(this OperationOutcome outcome, string message)
        {
            return outcome.AddIssue(OperationOutcome.IssueSeverity.Error, message);
        }

        public static OperationOutcome AddMessage(this OperationOutcome outcome, string message)
        {
            return outcome.AddIssue(OperationOutcome.IssueSeverity.Information, message);
        }

        public static OperationOutcome AddMessage(this OperationOutcome outcome, HttpStatusCode code, string message)
        {
            return outcome.AddIssue(IssueSeverityOf(code), message);
        }

        private static OperationOutcome AddIssue(this OperationOutcome outcome, OperationOutcome.IssueSeverity severity, string message)
        {
            if (outcome.Issue == null)
            {
                outcome.Init();
            }

            var item = new OperationOutcome.IssueComponent
            {
                Severity = severity,
                Diagnostics = message
            };
            outcome.Issue.Add(item);
            return outcome;
        }

        public static HttpResponseMessage ToHttpResponseMessage(this OperationOutcome outcome, ResourceFormat target)
        {
            // TODO: Remove this method is seems to not be in use.
            byte[] data = null;
            if (target == ResourceFormat.Xml)
            {
                var serializer = new FhirXmlSerializer();
                data = serializer.SerializeToBytes(outcome);
            }
            else if (target == ResourceFormat.Json)
            {
                var serializer = new FhirJsonSerializer();
                data = serializer.SerializeToBytes(outcome);
            }
            var response = new HttpResponseMessage
            {
                Content = new ByteArrayContent(data)
            };
            SetContentHeaders(response, target);

            return response;
        }
    }
}