namespace Spark.Engine.Web
{
    using System;
    using System.Net;
    using Core;
    using Engine.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Task = System.Threading.Tasks.Task;

    // https://stackoverflow.com/a/38935583
    public class ErrorHandler
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            Hl7.Fhir.Model.OperationOutcome outcome;
            if (exception is SparkException ex1)
            {
                code = ex1.StatusCode;
                outcome = GetOperationOutcome(ex1);
            }
            else
            {
                outcome = GetOperationOutcome(exception);
            }

            // Set HTTP status code
            context.Response.StatusCode = (int) code;
            OutputFormatterWriteContext writeContext = context.GetOutputFormatterWriteContext(outcome);
            IOutputFormatter formatter = context.SelectFormatter(writeContext);
            // Write the OperationOutcome to the Response using an OutputFormatter from the request pipeline
            await formatter.WriteAsync(writeContext).ConfigureAwait(false);
        }

        private Hl7.Fhir.Model.OperationOutcome GetOperationOutcome(SparkException exception)
        {
            if (exception == null) return null;
            return (exception.Outcome ?? new Hl7.Fhir.Model.OperationOutcome()).AddAllInnerErrors(exception);
        }

        private Hl7.Fhir.Model.OperationOutcome GetOperationOutcome(Exception exception)
        {
            return exception == null ? null : new Hl7.Fhir.Model.OperationOutcome().AddAllInnerErrors(exception);
        }
    }
}
