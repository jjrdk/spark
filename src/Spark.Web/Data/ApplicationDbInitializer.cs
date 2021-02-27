// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Web.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public static class ApplicationDbInitializer
    {
        public static void SeedAdmin(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IConfiguration config)
        {
            context.Database.Migrate();

            var adminEmail = config.GetValue<string>("Admin:Email");
            var adminPassword = config.GetValue<string>("Admin:Password");

            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var user = new IdentityUser {UserName = adminEmail, Email = adminEmail};

                var result = userManager.CreateAsync(user, adminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}