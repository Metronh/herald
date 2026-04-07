using UserService.AppSettings;
using UserService.Endpoints;
using UserService.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddLogging();
builder.AddSwagger();
builder.AddAppSettings();
builder.AddValidators();
builder.AddServices();
builder.RegisterAuthorization(builder.Configuration.GetSection("JwtTokenInformation").Get<JwtInformation>());

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

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