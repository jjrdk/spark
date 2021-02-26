namespace Spark.Engine.Web
{
    using System;
    using Handlers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class IApplicationBuilderExtensions
    {
        public static void UseFhir(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes = null)
        {
            app.UseMiddleware<ErrorHandler>();
            app.UseMiddleware<FormatTypeHandler>();
            app.UseMiddleware<MaintenanceModeHandler>();

            if (configureRoutes == null)
                app.UseMvc();
            else
                app.UseMvc(configureRoutes);
        }
    }
}
