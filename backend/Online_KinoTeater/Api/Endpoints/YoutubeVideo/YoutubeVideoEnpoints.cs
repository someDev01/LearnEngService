using Application.YoutubeVideo.Commands.AddYoutubeVideo;
using Application.YoutubeVideo.Commands.DeleteYoutubeVideo;
using Application.YoutubeVideo.Commands.UpdateYoutubeVideo;
using Application.YoutubeVideo.Queries.GetAllVideos;
using Application.YoutubeVideo.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.YoutubeVideo;

public static class YoutubeVideoEnpoints
{
    public static IEndpointRouteBuilder AddYoutubeVideoEnpoints(this IEndpointRouteBuilder app)
    {
        var youtubeVideos = app.MapGroup("api/youtubeVideos")
            .WithTags("Videos");

        var filterGroup = app.MapGroup("api/youtubeVideos")
            .WithTags("Search");

        #region ENDPOINTS YOUTUBEVIDEOS
        youtubeVideos.MapGet("/all", async (
            [FromQuery]int page,
            [FromQuery]int pageSize,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetYoutubeVideosQuery(page, pageSize));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        youtubeVideos.MapPost("/create", async ([FromBody] CreateYoutubeVideoCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });//.RequireAuthorization("AdminOnlyAccess");

        youtubeVideos.MapPatch("/update", async ([FromBody] UpdateYoutubeVideoCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result);
        }).RequireAuthorization("AdminOnlyAccess");

        youtubeVideos.MapDelete("/delete", async ([FromBody] DeleteYoutubeVideoCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.NoContent();
        });//.RequireAuthorization("AdminOnlyAccess");
        #endregion

        #region ENDPOINT SEARCH
        filterGroup.MapGet("/search", async (
            [FromQuery] string query,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new SearchVideosQuery(
                query,
                page,
                pageSize));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);  
        }).RequireAuthorization();
        #endregion

        return app;
    }
}
