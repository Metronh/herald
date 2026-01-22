using Microsoft.AspNetCore.Identity;
using UserService.Database;
using UserService.Endpoints;
using UserService.Entities;
using UserService.Extensions;
using UserService.Interfaces.Database;
using UserService.Interfaces.Repository;
using UserService.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddLogging();
builder.AddSwagger();
builder.AddAppSettings();
builder.AddValidators();
builder.AddServices();

var app = builder.Build();
app.UseCustomExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddHealthEndpoints();
app.AddUserEndpoints();
app.Run();