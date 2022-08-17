using Application.Enums;
using Application.Wrappers;
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
    public static class DefaultRoles
    {
        public static async Task<WebApplication> SeedDefaultRolesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    var superAdmin = await roleManager.FindByNameAsync(EnumRole.SuperAdmin.ToString());

                    if (superAdmin == null)
                    {
                        superAdmin = new IdentityRole(EnumRole.SuperAdmin.ToString());
                        await roleManager.CreateAsync(superAdmin);

                        for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(superAdmin, ClaimsStore.AllClaims[i]);
                        }
                    }

                    var voter = await roleManager.FindByNameAsync(EnumRole.Voter.ToString());

                    if (voter == null)
                    {
                        voter = new IdentityRole(EnumRole.Voter.ToString());
                        await roleManager.CreateAsync(voter);

                        for (int i = 0; i < ClaimsStore.VoterClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(voter, ClaimsStore.VoterClaims[i]);
                        }
                    }
                   
                }
            }

            return webApp;
        }


    }

    public static class ClaimsStore
    {
        private static readonly List<ClaimWrapper> allClaims = new()
        {
            new ClaimWrapper("candidate.read.policy", "candidate.read", "Read Candidates"),
            new ClaimWrapper("candidate.write.policy", "candidate.write", "Write Candidates"),
            new ClaimWrapper("candidate.manage.policy", "candidate.manage", "Manage Candidates"),

            new ClaimWrapper("category.read.policy", "category.read", "Read Categories"),
            new ClaimWrapper("category.write.policy", "category.write", "Write Categories"),
            new ClaimWrapper("category.manage.policy", "category.manage", "Manage Categories"),

            new ClaimWrapper("vote.read.policy", "vote.read", "Read Vote"),
            new ClaimWrapper("vote.write.policy", "vote.write", "Write Vote"),
            new ClaimWrapper("vote.manage.policy", "vote.manage", "Manage Vote"),

            new ClaimWrapper("voter.read.policy", "voter.read", "Read Voters"),
            new ClaimWrapper("voter.write.policy", "voter.write", "Write Voters"),
            new ClaimWrapper("voter.manage.policy", "voter.manage", "Manage Voters"),
        };

        private static readonly List<ClaimWrapper> voterClaims = new()
        {
            new ClaimWrapper("candidate.read.policy", "candidate.read", "Read Candidates"),

            new ClaimWrapper("category.read.policy", "category.read", "Read Categories"),

            new ClaimWrapper("vote.read.policy", "vote.read", "Read Vote"),
            new ClaimWrapper("vote.write.policy", "vote.write", "Write Vote"),
            new ClaimWrapper("vote.manage.policy", "vote.manage", "Manage Vote"),

            new ClaimWrapper("voter.read.policy", "voter.read", "Read Voters"),
            new ClaimWrapper("voter.write.policy", "voter.write", "Write Voters"),
        };

        public static List<ClaimWrapper> AllClaims = allClaims;
        public static List<ClaimWrapper> VoterClaims = voterClaims;
    }
}
