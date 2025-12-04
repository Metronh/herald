using ArticleService.AppSettings;
using ArticleService.Database;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Services;
using ArticleService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
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

    public static void RegisterServices(this WebApplicationBuilder builder) =>
        builder.Services.AddSingleton<IArticlesService, ArticlesService>();

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