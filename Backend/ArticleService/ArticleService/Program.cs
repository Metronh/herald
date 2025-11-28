using ArticleService.AppSettings;
using ArticleService.Database;
using ArticleService.Endpoints;
using ArticleService.Extensions;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Services;
using ArticleService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IMongoDbConnectionFactory, MongoDbConnectionFactory>();
builder.Services.AddSingleton<IArticlesService, ArticlesService>();

var app = builder.Build();
app.UseCustomExceptionHandling();
app.UseHttpsRedirection();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterGetArticleEndpoints();
app.AddHealthEndpoint();
app.Run();