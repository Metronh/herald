using ArticleService.AppSettings;
using ArticleService.Database;
using ArticleService.Endpoints;
using ArticleService.Extensions;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Services;
using ArticleService.RegisterServices;
using ArticleService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.RegisterSwagger();
builder.RegisterAppSettings();
builder.RegisterDatabases();
builder.RegisterServices();
builder.RegisterAuthorization(builder.Configuration.GetSection("JwtTokenInformation").Get<JwtInformation>());

var app = builder.Build();
app.UseCustomExceptionHandling();
app.UseHttpsRedirection();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.RegisterGetArticleEndpoints();
app.AddHealthEndpoint();
app.Run();