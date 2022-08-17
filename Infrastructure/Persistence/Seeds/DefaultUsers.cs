using Application.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static async Task<WebApplication> SeedSuperAdminAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Voter>>();
                var administrator = new Voter
                {
                    UserName = "superadmin@mail.com",
                    Email = "superadmin@mail.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var user = await userManager.FindByEmailAsync(administrator.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(administrator, "123Pa$$word!");
                    await userManager.AddToRoleAsync(administrator, EnumRole.SuperAdmin.ToString());
                }
            }

            return webApp;
        }
    }
}
