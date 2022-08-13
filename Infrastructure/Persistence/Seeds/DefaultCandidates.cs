using Application.Enums;
using Application.Features.Candidates.Queries.GetPagedList;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;

namespace Persistence.Seeds
{
    public static class DefaultCandidates
    {
        public static async Task<WebApplication> SeedDefaulCandidatesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var categoryRepository = scope.ServiceProvider.GetRequiredService<CategoryRepository>();
                var candidateRepository = scope.ServiceProvider.GetRequiredService<CandidateRepository>();


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


                foreach (var category in await candidateRepository.GetPagedListAsync(new GetCandidatesQuery { SearchTerm = "President" }))
                {

                    for (int i = 0; i < presidentsCandidatesToSeed.Length; i++)
                    {
                        var candidateMichael = new Candidate
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


                        if (!await candidateRepository.ExistAsync(candidateMichael))
                        {
                            await candidateRepository.CreateAsync(candidateMichael);
                        }
                    }
                }

                foreach (var category in await candidateRepository.GetPagedListAsync(new GetCandidatesQuery { SearchTerm = "Vice President" }))
                {

                    for (int i = 0; i < vicePresidentCandidatesToSeed.Length; i++)
                    {
                        var candidateMichael = new Candidate
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


                        if (!await candidateRepository.ExistAsync(candidateMichael))
                        {
                            await candidateRepository.CreateAsync(candidateMichael);
                        }
                    }
                }

                foreach (var category in await candidateRepository.GetPagedListAsync(new GetCandidatesQuery { SearchTerm = "Secretary" }))
                {

                    for (int i = 0; i < secretaryCandidatesToSeed.Length; i++)
                    {
                        var candidateMichael = new Candidate
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


                        if (!await candidateRepository.ExistAsync(candidateMichael))
                        {
                            await candidateRepository.CreateAsync(candidateMichael);
                        }
                    }
                }

            }

            return webApp;
        }
    }
}
