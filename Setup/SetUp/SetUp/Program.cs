
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using SetUp;
using SetUp.AppSettings;
using SetUp.Endpoints;
using SetUp.ExceptionMiddlewareExtensions;
using SetUp.Interfaces.Databases;
using SetUp.Interfaces.Repository;
using SetUp.Interfaces.Services;
using SetUp.Repository;
using SetUp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<CsvLocations>(builder.Configuration.GetSection("CsvLocations"));
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>();
builder.Services.AddSingleton<IMongoDbConnection, MongoDbConnection>();


var app = builder.Build();
app.UseCustomExceptionHandling();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Adding Endpoints here */
app.AddUploadEndpoints();
app.AddHealthEndpoints();

app.Run();