using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using SetUp.AppSettings;
using SetUp.Database;
using SetUp.Helpers;
using SetUp.Interfaces.Databases;
using SetUp.Interfaces.Helpers;
using SetUp.Interfaces.Repository;
using SetUp.Interfaces.Services;
using SetUp.Repository;
using SetUp.Services;

namespace SetUp.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IReadCsvHelper, ReadCsvHelper>();
        builder.Services.AddScoped<IUploadService, UploadService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
    }

    public static void AddAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
        builder.Services.Configure<CsvLocations>(builder.Configuration.GetSection("CsvLocations"));
    }

    public static void AddDatabases(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMongoDbConnection, MongoDbConnection>();
        builder.Services.AddDbContext<UsersDbContext>();
    }

    public static void AddSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(opt =>
        {
            opt.AddConsoleExporter();
            opt.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddAttributes(new Dictionary<string, object>()
            {
                ["deployment.environment"] = builder.Environment.EnvironmentName,
                ["service.name"] = "SetUp",
            }));
            opt.IncludeFormattedMessage = true;
            opt.IncludeFormattedMessage = true;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => 
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Set up application",
                Description = "This uploads dummy articles and users to the postgres and mongo DB",
            }));
    }
}