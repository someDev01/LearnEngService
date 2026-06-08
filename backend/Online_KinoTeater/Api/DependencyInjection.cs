using Api.Endpoints.Auth;
using Api.Endpoints.Note;
using Api.Endpoints.Subtitle;
using Api.Endpoints.Translate;
using Api.Endpoints.YoutubeVideo;
using Api.Middlewares;
using Api.Settings;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection IncludeSwaggerSpec(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Eng learning service",
                Description = "Learning Eng with enjoying!",
                Contact = new OpenApiContact
                {
                    Name = "Dmitry",
                    Email = "example@mail.ru",
                },
            });
        });

        return services;
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
    }

    public static IServiceCollection IncludeCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection(nameof(Cors));
        services.Configure<Cors>(corsSettings);
        string[] allowedOrigins = corsSettings.Get<Cors>()!.AllowedOrigins;

        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOnlyMyFrontend", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder app)
    {
        app.AddYoutubeVideoEnpoints();
        app.AddSubtitleEndpoints();
        app.AddEmailEnpoints();
        app.AddNoteEndpoints();
        app.AddTranslateEndpoints();
        return app;
    }

    public static IServiceCollection IncludeMediatR(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith("Application"))
            .ToArray();

        services.AddMediatR(options => options.RegisterServicesFromAssemblies(assemblies));

        return services;
    }

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();

        return app;
    }

}
