using Microsoft.AspNetCore.Identity;
using Application.Enums;
using System.Security.Claims;
using Application.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using Domain.Entities;
using Persistence.Contexts;
using Application.Interfaces;

namespace Persistence.Seeds
{
    public static class DefaultCategories
    {
        public static async Task<WebApplication> SeedDefaultCategoriesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
                var nestClient = scope.ServiceProvider.GetRequiredService<Nest.ElasticClient>();

                string[] categoriesToSeed =
                {
                    "President",
                    "Vice President",
                    "Secretary",
                    "Mon role tout beau tout propre",
                };

                for (int i = 0; i < categoriesToSeed.Length; i++)
                {
                    var category = new Category
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        UpdatedBy = null,
                        Name = categoriesToSeed[i],
                    };

                    if (!await repository.Category.ExistAsync(category))
                    {
                        var response = await nestClient.IndexAsync(category,
                            x => x.Index(EnumElasticIndexes.Categories.ToString())
                        );
                        category.Id = response.Id;
                        await repository.Category.CreateAsync(category);
                    }
                }

                await repository.SaveAsync();
            }

            return webApp;
        }
    }
}
