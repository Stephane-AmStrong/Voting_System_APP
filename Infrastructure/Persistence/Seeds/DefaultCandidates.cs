using Application.Enums;
using Application.Features.Candidates.Queries.GetPagedList;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repository;

namespace Persistence.Seeds
{
    public static class DefaultCandidates
    {
        public static async Task<WebApplication> SeedDefaulCandidatesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                var presidentsCandidatesToSeed = new string[,]
                {
                    { "Michael","Scott" },
                    { "Andy","Bernard" },
                    { "Deangelo","Vickers" },
                };
                var vicePresidentCandidatesToSeed = new string[,]
                {
                    { "Dwight","Schrute" },
                    { "John","Halpert" },
                };
                var secretaryCandidatesToSeed = new string[,]
                {
                    { "Pam","Beasly" },
                    { "Erin","Hannon" },
                    { "Kevin","Malone" },
                };


                foreach (var category in repository.Categories.Where(x=> x.Name == "President").ToList())
                {

                    for (int i = 0; i < presidentsCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = presidentsCandidatesToSeed[i, 0],
                            LastName = presidentsCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };

                        if (!repository.Candidates.Any(x=> x.FirstName == candidate.FirstName && x.LastName == candidate.LastName)) repository.Candidates.Add(candidate);
                    }
                }

                foreach (var category in repository.Categories.Where(x => x.Name == "Vice President").ToList())
                {

                    for (int i = 0; i < vicePresidentCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = vicePresidentCandidatesToSeed[i, 0],
                            LastName = vicePresidentCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };


                        if (!repository.Candidates.Any(x => x.FirstName == candidate.FirstName && x.LastName == candidate.LastName)) repository.Candidates.Add(candidate);
                    }
                }

                foreach (var category in repository.Categories.Where(x => x.Name == "Secretary").ToList())
                {

                    for (int i = 0; i < secretaryCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = secretaryCandidatesToSeed[i, 0],
                            LastName = secretaryCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };


                        if (!repository.Candidates.Any(x => x.FirstName == candidate.FirstName && x.LastName == candidate.LastName)) repository.Candidates.Add(candidate);
                    }
                }

                await repository.SaveChangesAsync();

            }

            return webApp;
        }
    }
}
