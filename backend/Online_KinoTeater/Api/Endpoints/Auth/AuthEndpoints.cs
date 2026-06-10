using Api.Filters;
using Application.Auth.Commands.Revoke;
using Application.Auth.Commands.Send;
using Application.Auth.Commands.Verify;
using Application.Auth.Commands.VerifyAdmin;
using Application.Auth.Queries.GetUserById;
using Application.Common.Claims;
using Infrastructure.Settings.Jwt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder AddEmailEnpoints(
        this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api/auth")
            .WithTags("Auth");

        var authGroupAdmin = app.MapGroup("/api/admin-auth")
            .WithTags("Admin-Auth");

        #region EMAIL ENDPOINTS
        authGroup.MapPost("send-code", async ([FromBody] SendVerificationCodeCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(new
            {
                isSended = result.IsSuccess,
                time = result.Value,
                step = "verify"
            });
        });

        authGroup.MapPost("verify-code", async (
            [FromBody] VerificationCodeCommand command,
            IMediator mediator,
            [FromServices] IOptions<JwtSettings> options,
            HttpContext context) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            var jwt = result.Value!.Jwt;

            context.Response.Cookies.Append(options.Value.TokenName, jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(options.Value.TokenExpireUserDays)
            });
            
            return Results.Ok(new
            {
                isVerified = result.IsSuccess,
                email = result.Value.Email,
                step = "done"
            });
        });

        authGroupAdmin.MapPost("verify-code-admin", async (
            [FromBody] VerificationCodeAndSecretAdminCommand command,
            IMediator mediator,
            [FromServices] IOptions<JwtSettings> options,
            HttpContext context) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            context.Response.Cookies.Append(options.Value.TokenName, result.Value!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(options.Value.TokenExpireAdminHours)
            });

            return Results.Ok(new
            {
                isVerified = result.IsSuccess,
                step = "done"
            });
        }).AddEndpointFilter<AdminHeaderKeyFilter>();
            
        #endregion

        #region AUTH ENDPOINTS

        authGroup.MapGet("me", async (ClaimsPrincipal claims, IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new GetUserByIdQuery(userId));
            if (!result.IsSuccess)
                return Results.BadRequest();

            return Results.Ok(new
            {
                email = result.Value!.Email,
            });
        }).RequireAuthorization();

        authGroup.MapPost("logout", async (HttpContext context, [FromServices] IOptions<JwtSettings> options, IMediator mediator) =>
        {
            var jti = context.User.GetJti();

            var expires = context.User.GetTokenExpireTime();
            var ttl = expires - DateTime.UtcNow;

            var command = new RevokeJwtCommand(jti, ttl);
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            context.Response.Cookies.Delete(jwtSettings!.TokenName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });

            return Results.Ok(result);
        }).RequireAuthorization();
        #endregion

        return app;
    }
}
