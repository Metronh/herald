using ArticleService.AppSettings;
using ArticleService.Database;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Repository;
using ArticleService.Interfaces.Services;
using ArticleService.Repository;
using ArticleService.Services;
using ArticleService.Services.CachedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ArticleService.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void RegisterSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter token here",
                Name = "Auth",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public static void RegisterAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
        builder.Services.Configure<JwtInformation>(
            builder.Configuration.GetSection("JwtTokenInformationJwtTokenInformation"));
    }

    public static void RegisterDatabases(this WebApplicationBuilder builder) =>
        builder.Services.AddSingleton<IMongoDbConnectionFactory, MongoDbConnectionFactory>();

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ArticlesService>();
        builder.Services.AddSingleton<IArticlesService>(x =>
            new CachedArticleService(x.GetRequiredService<ArticlesService>(), x.GetRequiredService<HybridCache>(),
                x.GetRequiredService<ILogger<CachedArticleService>>()));
        builder.Services.AddSingleton<IArticlesRepository, ArticlesRepository>();
    }

    public static void AddCaching(this WebApplicationBuilder builder, ConnectionStrings connectionStrings)
    {
        builder.Services.AddStackExchangeRedisCache(opt => { opt.Configuration = connectionStrings.Redis; });
        builder.Services.AddHybridCache();
    }

    public static void RegisterAuthorization(this WebApplicationBuilder builder, JwtInformation jwtInfo)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtInfo.TokenKey)),
                ValidIssuer = jwtInfo.Issuer,
                ValidAudience = jwtInfo.Audience,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
            };
        });
    }
}