using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WebApi.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {

                // Set the comments path for the Swagger JSON and UI.
                c.IncludeXmlComments(string.Format(@"{0}\wwwroot\WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));
                c.SchemaFilter<EnumSchemaFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Voting_System API",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "Stéphane Adjakotan",
                        Email = "stephane.adjakotan@gmail.com",
                        Url = new Uri("https://github.com/Stephane-AmStrong"),
                    }
                });

                c.AddSecurityDefinition("Bearer", OpenApiOperationFilter.SecuritySchema);

                c.OperationFilter<OpenApiOperationFilter>();
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }
    }


    public class OpenApiOperationFilter : IOperationFilter
    {
        private static OpenApiSecurityScheme securitySchema = new OpenApiSecurityScheme
        {
            Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer",
            },
        };

        public static OpenApiSecurityScheme SecuritySchema
        {
            get { return securitySchema; }
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
            !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            securitySchema, Array.Empty<string>()
                        }
                    }
                };
            }

        }
    }

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(n => model.Enum.Add(new OpenApiString(n)));
            }
        }
    }


}
