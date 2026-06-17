using Application.User.Queries.GetAllUsers;
using MediatR;

namespace Api.Endpoints.User;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/api/user")
            .WithTags("Users");

        userGroup.MapGet("all", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUsersQuery());
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        }).RequireAuthorization("AdminOnlyAccess");

        return app;
    }
}