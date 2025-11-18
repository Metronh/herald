using Microsoft.EntityFrameworkCore;
using UploadData.AppSettings;
using UploadData.Endpoints;
using UploadData.Interfaces;
using UploadData.Interfaces.Services;
using UploadData.Repository.Context;
using UploadData.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));
builder.Services.Configure<ArticlesLocation>(builder.Configuration.GetSection("ArticlesLocation"));
builder.Services.Configure<MongoDb>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<UserDataLocation>(builder.Configuration.GetSection("UserDataLocation"));
builder.Services.Configure<PostgresDb>(builder.Configuration.GetSection("PostgresDb"));
builder.Services.AddTransient<IReadArticlesService, ReadFilesService>();
builder.Services.AddScoped<IUploadService, UploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Adding Endpoints here */
app.AddUploadEndpoints();

app.UseHttpsRedirection();

app.Run();