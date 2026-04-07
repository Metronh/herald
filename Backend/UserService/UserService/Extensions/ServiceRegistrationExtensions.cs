using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using UserService.AppSettings;
using UserService.Database;
using UserService.Entities;
using UserService.Interfaces.Database;
using UserService.Interfaces.Repository;
using UserService.Interfaces.Services;
using UserService.Messaging.Events;
using UserService.Middleware;
using UserService.Repository;
using UserService.Services;
using UserService.Validation;

namespace UserService.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddHostedService<SessionCleanUpService>();
        builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        builder.Services.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();
            busConfig.UsingRabbitMq((context, config) =>
            {
                config.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(builder.Configuration["MessageBroker:Username"]!);
                    host.Password(builder.Configuration["MessageBroker:Password"]!);
                });
                config.Message<SendWelcomeEmailEvent>(x =>
                {
                    x.SetEntityName("UserService.Messaging.Events:SendWelcomeEmailEvent");
                });
                config.Publish<SendWelcomeEmailEvent>(x =>
                {
                    x.BindQueue("UserService.Messaging.Events:SendWelcomeEmailEvent", "resend-queue", b =>
                    {
                        b.Durable = true;
                        b.AutoDelete = false;
                    });
                });
                config.ConfigureEndpoints(context);
            });
        }); 
    }

    public static void AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CreateUserRequestValidator>();
        builder.Services.AddScoped<LoginRequestValidator>();
        builder.Services.AddScoped<DeactivateAccountRequestValidator>();
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
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

    public static void AddAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<ConnectionStrings>().BindConfiguration("ConnectionStrings")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>().Value);
        builder.Services.AddOptions<JwtInformation>().BindConfiguration("JwtTokenInformation")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<JwtInformation>>().Value);
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(opt =>
        {
            opt.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddAttributes(
                new Dictionary<string, object>
                {
                    ["deployment.environment"] = builder.Environment.EnvironmentName,
                    ["service.name"] = "UserService",
                }));
            opt.IncludeScopes = true;
            opt.IncludeFormattedMessage = true;
            opt.AddConsoleExporter();
        });
    }
    
    public static void RegisterAuthorization(this WebApplicationBuilder builder, JwtInformation jwtInfo)
    {
        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Admin", policy => { policy.RequireRole("Admin"); });
        });
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