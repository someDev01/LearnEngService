using System.Security.Claims;
using Application.Common.Claims;
using Application.Profile.Queries.GetProfile;
using MediatR;

namespace Api.Endpoints.Profile;

public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder AddProfileEndpoint(this IEndpointRouteBuilder app)
    {
        var groupProfile = app.MapGroup("profile")
            .WithTags("Profile");
        
        groupProfile.MapGet("/", async (ClaimsPrincipal claim, IMediator mediator) =>
        {
            var userId = claim.GetUserId();

            var result = await mediator.Send(new GetProfileQuery(userId));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Value);
            return Results.Ok(result.Value);

        }).RequireAuthorization();
        
        return app;
    }
}