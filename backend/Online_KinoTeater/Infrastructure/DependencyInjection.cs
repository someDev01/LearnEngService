using Amazon.S3;
using Application.Auth.Commands.Send;
using Application.Auth.Commands.Verify;
using Application.Auth.Commands.VerifyAdmin;
using Application.Common.Cache;
using Application.Configs.Admin;
using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.CacheService;
using Application.Interfaces.Clients.Llm;
using Application.Interfaces.Clients.Youtube;
using Application.Interfaces.Code;
using Application.Interfaces.Context;
using Application.Interfaces.Email;
using Application.Interfaces.NoteCache;
using Application.Interfaces.NotePolicy;
using Application.Interfaces.NoteRead;
using Application.Interfaces.RoleAdmin;
using Application.Interfaces.Storage;
using Application.Interfaces.Token;
using Application.Interfaces.Translate;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.VerificationCode;
using Application.Interfaces.VideoCache;
using Application.Interfaces.VideoRead;
using Application.Interfaces.Vocabulary;
using Application.Note.Commands.CreateNote;
using Application.Services.AuthVerificationPolicy;
using Application.Services.NoteCache;
using Application.Services.NotePolicy;
using Application.Services.NoteRead;
using Application.Services.VerificationCode;
using Application.Services.VideoCache;
using Application.Services.VideoRead;
using Application.Services.Vocabulary;
using Application.Settings.Cache;
using Application.Settings.Code;
using Application.Settings.RateLimit;
using Application.Subtitle.Commands.CreateSubtitle;
using Application.Subtitle.Commands.UpdateSubtitle;
using Application.Translate.Commands.Translate;
using Application.Validators.CreateNote;
using Application.Validators.CreateSubtitle;
using Application.Validators.SendVerify;
using Application.Validators.TranslateWord;
using Application.Validators.UpdateSubtitle;
using Application.Validators.VerifyCode;
using Application.Validators.VerifyCodeAdmin;
using Domain.Repositories.Note;
using Domain.Repositories.Subtitle;
using Domain.Repositories.User;
using Domain.Repositories.Video;
using FluentValidation;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Repositories.Note;
using Infrastructure.Repositories.Subtitle;
using Infrastructure.Repositories.User;
using Infrastructure.Repositories.Video;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Code;
using Infrastructure.Services.Email.Render;
using Infrastructure.Services.Email.Smtp;
using Infrastructure.Services.Jwt;
using Infrastructure.Services.Llm;
using Infrastructure.Services.RoleInit;
using Infrastructure.Services.Storage;
using Infrastructure.Services.Translate;
using Infrastructure.Services.Youtube;
using Infrastructure.Settings.Email;
using Infrastructure.Settings.Jwt;
using Infrastructure.Settings.Llm;
using Infrastructure.Settings.Storage;
using Infrastructure.Settings.Translate;
using Infrastructure.Settings.Youtube;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this  IServiceCollection services, IConfiguration configuration)
    {
        #region CONFIGURATIONS
        var jwtSettings = configuration.GetSection(nameof(JwtSettings));
        services.Configure<JwtSettings>(jwtSettings);
        services.Configure<EmailSettings>(configuration.GetSection("Email"));
        services.Configure<S3Settings>(configuration.GetSection("S3Storage"));

        services.Configure<CacheTtlSettings>(configuration.GetSection("CacheTtl"));

        services.Configure<CodeSettings>(configuration.GetSection("RateLimit:CodeVerify"));

        services.Configure<AdminSettings>(configuration.GetSection("Admin"));

        services.Configure<YoutubeApiSettings>(configuration.GetSection("YoutubeApi"));
        services.Configure<LlmSettings>(configuration.GetSection("Llm"));

        services.Configure<MyMemoryTranslationSettings>(configuration.GetSection("MyMemoryTranslate"));

        services.Configure<RateLimitSettings>(configuration.GetSection("RateLimit"));
        #endregion

        #region PERSISTENCES
        services.AddDbContext<DataContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDataContext, DataContext>();

        #endregion

        #region REPOSITORIES
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ISubtitleRepository, SubtitleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        #endregion

        #region SERVICES
        services.AddRedisService(configuration);
        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddScoped<ITokenService, JwtService>();
        services.AddJwtAuth(Options.Create(jwtSettings.Get<JwtSettings>()!));
        services.AddScoped<ICodeGenerationService, CodeGenerationService>();

        services.AddScoped<IEmailService, ResendEmailService>();
        services.AddScoped<IEmailTemplateRender, EmailTemplateRender>();

        services.AddScoped<IAdminService, AdminService>();

        services.AddScoped<IVocabularyService, VocabularyService>();

        services.AddScoped<INotePolicyService, NotePolicyService>();

        services.AddScoped<IAuthVerificationPolicyService, AuthVerificationPolicyService>();
        services.AddScoped<IVerificationCodeService, VerificationCodeService>();

        services.AddScoped<INoteReadService, NoteReadService>();
        services.AddScoped<IVideoReadService, VideoReadService>();

        services.AddScoped<INoteCacheService, NoteCacheService>();
        services.AddScoped<IVideoCacheService, VideoCacheService>();

        services.AddHttpClient<IEmailService, ResendEmailService>(options =>
        {
            options.BaseAddress = new Uri(configuration["Email:BaseUrl"]!);
            options.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["Email:ResendApiKey"]}");
        });

        services.AddHttpClient<ILlmClient, GroqClient>(options =>
        {
            options.BaseAddress = new Uri(configuration["Llm:BaseUrl"]!);
            options.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["Llm:ApiKey"]}");
        });

        services.AddHttpClient<ITranslateService, MyMemoryTranslateService>((sp, options) =>
        {
            var settings = sp.GetRequiredService<IOptions<MyMemoryTranslationSettings>>().Value;
            options.BaseAddress = new Uri(settings.BaseUrl);
        });

        services.AddHttpClient<IYoutubeClient, YoutubeClient>((sp, options) =>
        {
            var settings = sp.GetRequiredService<IOptions<YoutubeApiSettings>>().Value;
            options.BaseAddress = new Uri(settings.BaseUrl);
        });

        services.AddScoped<IFileStorageService, S3StorageService>();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;

            var config = new AmazonS3Config
            {
                ServiceURL = settings.EndpointUrl,
                ForcePathStyle = true
            };

            return new AmazonS3Client(
                settings.AccessKey,
                settings.SecretKey,
                config);
        });

        #endregion

        #region VALIDATORS
        services.AddScoped<IValidator<SendVerificationCodeCommand>, SendVerificationsCodeCommandValidator>();
        services.AddScoped<IValidator<VerificationCodeCommand>, VerificationCodeCommandValidator>();
        services.AddScoped<IValidator<VerificationCodeAndSecretAdminCommand>, VerificationCodeAdminCommandValidator>();

        services.AddScoped<IValidator<CreateNoteCommand>, CreateNoteCommandValidator>();
        services.AddScoped<IValidator<CreateNoteWithContextCommand>, CreateNoteWithContextCommandValidator>();
        services.AddScoped<IValidator<CreateSubtitleCommand>, CreateSubtitleCommandValidator>();
        services.AddScoped<IValidator<UpdateSubtitleCommand>, UpdateSubtitleCommandValidator>();
        services.AddScoped<IValidator<TranslateWordCommand>, TranslateWordCommandValidator>();
        #endregion

        return services;
    }

    public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string config = configuration.GetConnectionString("Redis")!;
            return ConnectionMultiplexer.Connect(config);
        });

        return services;
    }

    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services, IOptions<JwtSettings> settings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings.Value.SecretKey.ToString())),

                    ValidateIssuer = true,
                    ValidIssuer = settings.Value.Issuer,

                    ValidateAudience = true,
                    ValidAudience = settings.Value.Audience,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    NameClaimType = ClaimTypes.NameIdentifier,

                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["__Secure-token"];
                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var connection = context.HttpContext.RequestServices
                            .GetRequiredService<IConnectionMultiplexer>();

                        var redis = connection.GetDatabase();

                        var jti = context.Principal?
                            .FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                        if (string.IsNullOrEmpty(jti)) return;

                        var revoked = await redis.KeyExistsAsync(CacheKeyBuilder.BuildRevokeKey(jti.ToString()));
                        if (revoked)
                        {
                            context.Fail("Токен уже отозван");
                            return;
                        }
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var loggerFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>();

                        var logger = loggerFactory.CreateLogger("Jwt");
                        logger.LogWarning($"Ошибка проверки JWT {context.Exception.Message}");

                        context.Fail("Invalid Token");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;

                        if(context.Request.Cookies.TryGetValue("__Secure-token", out _))
                        {
                            context.Response.Cookies.Append("__Secure-token", "", new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddDays(-1)
                            });
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnlyAccess", policy =>
            {
                policy.RequireRole("Admin");
            });
        return services;
    }
}

