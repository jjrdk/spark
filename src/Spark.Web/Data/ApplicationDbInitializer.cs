using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Spark.Web.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedAdmin(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            context.Database.Migrate();

            var adminEmail = config.GetValue<string>("Admin:Email");
            var adminPassword = config.GetValue<string>("Admin:Password");

            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = userManager.CreateAsync(user, adminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}