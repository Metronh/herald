using System.Text.Json;
using CommunicationsFunctions.AppSettings;
using CommunicationsFunctions.Database;
using CommunicationsFunctions.Interfaces.Database;
using CommunicationsFunctions.Interfaces.Repository;
using CommunicationsFunctions.Interfaces.Services;
using CommunicationsFunctions.Middleware;
using CommunicationsFunctions.Repository;
using CommunicationsFunctions.Services;
using CommunicationsFunctions.Validators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Polly;
using Polly.Retry;
using Resend;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// AppSettings
builder.Services.AddOptions<ConnectionStrings>().BindConfiguration("ConnectionStrings").ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<ConnectionStrings>(sp => sp.GetRequiredService<IOptions<ConnectionStrings>>().Value);
builder.Services.AddOptions<ResendSettings>().BindConfiguration("ResendSettings").ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<ResendSettings>(sp => sp.GetRequiredService<IOptions<ResendSettings>>().Value);

// Services 
builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();

// Validators
builder.Services.AddScoped<SendEmailRequestValidator>();

//Resend
builder.Services.AddHttpClient<ResendClient>();
builder.Services.AddOptions<ResendClientOptions>()
    .Configure<ResendSettings>((opt, settings) => { opt.ApiToken = settings.ApiKey; });
builder.Services.AddTransient<IResend, ResendClient>();


// Logging
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(opt =>
    {
        opt.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddAttributes(new Dictionary<string, object>
        {
            ["deployment.environment"] = builder.Environment.EnvironmentName,
            ["service.name"] = "CommunicationsFunction",
        }));
        opt.IncludeScopes = true;
        opt.IncludeFormattedMessage = true;
        opt.AddConsoleExporter();
    }
);

builder.Services.AddResiliencePipeline("ResendPipeline", x =>
{
    x.AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        Delay = TimeSpan.FromSeconds(2),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    }).AddTimeout(TimeSpan.FromSeconds(30));
});

// Json Serializer
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
});


// Middleware
builder.UseMiddleware<ErrorHandlingMiddleware>();
//Move to before logging
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();