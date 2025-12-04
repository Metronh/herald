using Microsoft.OpenApi.Models;
using UploadData.AppSettings;
using UploadData.Database;
using UploadData.Endpoints;
using UploadData.Extensions;
using UploadData.Interfaces.Database;
using UploadData.Interfaces.Repository;
using UploadData.Interfaces.Services;
using UploadData.Repository;
using UploadData.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.AddSingleton<IMongoDbConnectionFactory, MongoDbConnectionFactory>();


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