using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public static class IdentitySeedData
    {
        private const string _adminUser = "Admin";
        private const string _adminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<AppIdentityDbContext>();

            //Check for pending migrations
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            UserManager<IdentityUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(_adminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin")
                {
                    Email = "admin@example.com",
                    PhoneNumber = "555-1234"
                };
                await userManager.CreateAsync(user, _adminPassword);
            }
        }
    }
}
