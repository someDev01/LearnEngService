using Api;
using Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.IncludeCors(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.IncludeMediatR();

builder.Logging.AddConsole(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.IncludeSwaggerSpec();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplyMigrations();
//app.UseHttpsRedirection();
app.UseCors("AllowedOnlyMyFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomMiddleware();
app.AddEndpoints();

app.Run();
