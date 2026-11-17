using System.Text.Json.Serialization;
using Api;
using Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.PropertyNameCaseInsensitive = true;   
});

builder.Services.IncludeCors(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.IncludeMediatR();

builder.Logging.AddConsole(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.IncludeSwaggerSpec();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
}

await app.ApplyMigrations();
//app.UseHttpsRedirection();
app.UseCors("AllowedOnlyMyFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomMiddleware();
app.AddEndpoints();

app.Run();
