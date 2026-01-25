using SetUp.Endpoints;
using SetUp.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();
builder.AddAppSettings();
builder.AddDatabases();
builder.AddServices();
var app = builder.Build();
app.UseCustomExceptionHandling();
app.UseHttpsRedirection();
app.AddSwagger();

/* Adding Endpoints here */
app.AddUploadEndpoints();
app.AddHealthEndpoints();
app.AddDeleteEndpoints();

app.Run();