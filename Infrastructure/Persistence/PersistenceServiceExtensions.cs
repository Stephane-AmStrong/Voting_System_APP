using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Persistence.Repository;
using Persistence.Repositories;
using Application.Models;

namespace Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer
                (
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
            );

            services.AddScoped<ISortHelper<Candidate>, SortHelper<Candidate>>();
            services.AddScoped<ISortHelper<Category>, SortHelper<Category>>();
            services.AddScoped<ISortHelper<Voter>, SortHelper<Voter>>();
            services.AddScoped<ISortHelper<Vote>, SortHelper<Vote>>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
