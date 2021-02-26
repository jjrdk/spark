﻿namespace Spark.Web
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Spark.Web.Data;

    using Microsoft.AspNetCore.Server.Kestrel.Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    ApplicationDbInitializer.SeedAdmin(context, userManager, config);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureKestrel(
                    k =>
                    {
                        k.AddServerHeader = false;
                        k.ListenAnyIP(80, options => options.Protocols = HttpProtocols.Http1AndHttp2);
                    })
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                });
    }
}