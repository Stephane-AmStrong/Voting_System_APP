using Application.Interfaces;
using Domain.Settings;
using Persistence.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Elasticsearch.Net;
using Nest;

namespace Persistence
{
    public static class ElasticSearchServiceExtensions
    {
        public static void AddElasticSearchServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration["ElasticSearchSettings:Url"]));
            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}
