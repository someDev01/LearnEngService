using System.Security.Claims;
using Application.Common.Claims;
using Application.User.Commands.UploadAvatar;
using Application.User.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints.User;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("user")
            .WithTags("Users");

        userGroup.MapGet("all", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUsersQuery());
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        }).RequireAuthorization("AdminOnlyAccess");

        userGroup.MapPost("upload-avatar", async (
            IFormFile file,
            ClaimsPrincipal claims,
            IMediator mediator) =>
        {   
            if(file is null)
                return Results.BadRequest("Файл обязателен");
            
            await using var stream = file.OpenReadStream();
            
            var userId = claims.GetUserId();
            var result = await mediator.Send(new UploadAvatarCommand(
                userId,
                stream,
                file.FileName,
                file.Length,
                file.ContentType));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(new
            {
                avatarUrl = result.Value
            });
        }).RequireAuthorization()
            .DisableAntiforgery();
        
        return app;
    }
}