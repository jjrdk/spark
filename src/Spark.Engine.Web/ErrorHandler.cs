using FhirModel = Hl7.Fhir.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Spark.Engine.Core;
using Spark.Engine.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Spark.Engine.ExceptionHandling
{
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
            FhirModel.OperationOutcome outcome;
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

        private FhirModel.OperationOutcome GetOperationOutcome(SparkException exception)
        {
            if (exception == null) return null;
            return (exception.Outcome ?? new FhirModel.OperationOutcome()).AddAllInnerErrors(exception);
        }

        private FhirModel.OperationOutcome GetOperationOutcome(Exception exception)
        {
            return exception == null ? null : new FhirModel.OperationOutcome().AddAllInnerErrors(exception);
        }
    }
}
