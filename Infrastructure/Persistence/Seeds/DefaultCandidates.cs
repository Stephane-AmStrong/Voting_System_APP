using Application.Enums;
using Application.Features.Candidates.Queries.GetPagedList;
using Application.Features.Categories.Queries.GetPagedList;
using Application.Interfaces;
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
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
                var nestClient = scope.ServiceProvider.GetRequiredService<Nest.ElasticClient>();


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


                foreach (var category in await repository.Category.GetPagedListAsync(new GetCategoriesQuery { WithName = "President"}))
                {

                    for (int i = 0; i < presidentsCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = presidentsCandidatesToSeed[i, 0],
                            LastName = presidentsCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };

                        if (! await repository.Candidate.ExistAsync(candidate))
                        {
                            var response = await nestClient.IndexAsync(candidate,
                                x => x.Index(EnumElasticIndexes.Candidates.ToString())
                            );

                            candidate.Id = response.Id;
                            await repository.Candidate.CreateAsync(candidate);
                        }
                    }
                }

                foreach (var category in await repository.Category.GetPagedListAsync(new GetCategoriesQuery { WithName = "Vice President" }))
                {

                    for (int i = 0; i < vicePresidentCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = vicePresidentCandidatesToSeed[i, 0],
                            LastName = vicePresidentCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };


                        if (!await repository.Candidate.ExistAsync(candidate))
                        {
                            var response = await nestClient.IndexAsync(candidate,
                                x => x.Index(EnumElasticIndexes.Candidates.ToString())
                            );

                            candidate.Id = response.Id;
                            await repository.Candidate.CreateAsync(candidate);
                        }
                    }
                }

                foreach (var category in await repository.Category.GetPagedListAsync(new GetCategoriesQuery { WithName = "Secretary"}))
                {

                    for (int i = 0; i < secretaryCandidatesToSeed.GetLength(0); i++)
                    {
                        var candidate = new Candidate
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedBy = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = null,
                            UpdatedBy = null,
                            FirstName = secretaryCandidatesToSeed[i, 0],
                            LastName = secretaryCandidatesToSeed[i, 1],
                            CategoryId = category.Id,
                        };


                        if (!await repository.Candidate.ExistAsync(candidate))
                        {
                            var response = await nestClient.IndexAsync(candidate,
                                x => x.Index(EnumElasticIndexes.Candidates.ToString())
                            );

                            candidate.Id = response.Id;
                            await repository.Candidate.CreateAsync(candidate);
                        }
                    }
                }

                await repository.SaveAsync();

            }

            return webApp;
        }
    }
}
