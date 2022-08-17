﻿using Microsoft.AspNetCore.Identity;
using Application.Enums;
using System.Security.Claims;
using Application.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Seeds
{
    public static class DefaultCategories
    {
        public static async Task<WebApplication> SeedDefaultCategoriesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var categoryRepository = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                string[] categoriesToSeed =
                {
                    "President",
                    "Vice President",
                    "Secretary",
                };

                for (int i = 0; i < categoriesToSeed.Length; i++)
                {
                    var category = new Category
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = null,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        UpdatedBy = null,
                        Name = categoriesToSeed[i],
                    };

                    if (!categoryRepository.Categories.Any(x=>x.Name == category.Name)) categoryRepository.Categories.Add(category);
                }

                await categoryRepository.SaveChangesAsync();
            }

            return webApp;
        }
    }
}
