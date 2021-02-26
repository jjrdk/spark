namespace Spark.Engine.Web
{
    using System.Threading.Tasks;
    using Maintenance;
    using Microsoft.AspNetCore.Http;

    public class MaintenanceModeHandler
    {
        private readonly RequestDelegate _next;

        public MaintenanceModeHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (MaintenanceMode.IsEnabledForHttpMethod(context.Request.Method))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }
            await _next(context).ConfigureAwait(false);
        }
    }
}
