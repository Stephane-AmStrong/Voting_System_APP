using Application;
using Application.Interfaces;
using Application.Mappings;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Persistence;
using Persistence.Seeds;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using WebApi.Extensions;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    // uncomment to write to Azure diagnostics stream
    .WriteTo.File
    (
        $"{environment.WebRootPath}/logfiles/logfile-.txt",
        rollingInterval: RollingInterval.Day
    )
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.ConfigureJwtBearerService(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddElasticSearchServiceConfiguration(builder.Configuration);
builder.Services.AddSwaggerExtension();

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.Converters.Add(new StringEnumConverter
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    });
});

builder.Services.ConfigureClaimPolicy();
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

try
{
    Log.Information("Starting Seeding Default Roles");
    await app.SeedDefaultRolesAsync();
    Log.Information("Roles Seeding Complete");
    
    Log.Information("Starting Seeding SuperAdmin");
    await app.SeedSuperAdminAsync();
    Log.Information("SuperAdmin Seeding Complete");
    
    Log.Information("Starting Seeding Default Categories");
    await app.SeedDefaultCategoriesAsync();
    Log.Information("Categories Seeding Complete");

    Log.Information("Starting Seeding Default Candidates");
    await app.SeedDefaulCandidatesAsync();
    Log.Information("Candidates Seeding Complete");

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwaggerExtension();
    app.UseErrorHandlingMiddleware();
    app.UseHealthChecks("/health");
    app.MapControllers();

    app.Run();

    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");

    return 1;
}
finally
{
    Log.CloseAndFlush();
}
