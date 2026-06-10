using Api.Requests;
using Application.Subtitle.Commands.CreateSubtitle;
using Application.Subtitle.Commands.DeleteSubtitle;
using Application.Subtitle.Commands.UpdateSubtitle;
using Application.Subtitle.Queries.GetAllSubtitles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Subtitle;

public static class SubtitleEndpoints
{
    public static IEndpointRouteBuilder AddSubtitleEndpoints(this IEndpointRouteBuilder app)
    {
        var subtitleGroup = app.MapGroup("api/subtitles")
            .WithTags("Subtitles");

        var videoPlayerGroup = app.MapGroup("api/videoPlayer")
            .WithTags("VideoPlayer");

        #region ENDPOINTS SUBTITLE
        videoPlayerGroup.MapGet("get/by/videoId", async ([FromQuery] Guid videoId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetVideoPlayerByVideoIdQuery(videoId));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);
            return Results.Ok(result.Value);
        }).RequireAuthorization();

        subtitleGroup.MapGet("/by-videoId", async (
            [FromQuery] Guid videoId,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSubtitlesVideoQuery(videoId));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        }).RequireAuthorization("AdminOnlyAccess");

        subtitleGroup.MapPost("/create", async (
            [FromForm] CreateSubtitlesRequest request,
            IFormFile subtitles,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSubtitleCommand(
                request.VideoId,
                request.Language,
                subtitles.OpenReadStream(),
                subtitles.FileName,
                request.Format,
                subtitles.ContentType));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok();
        }).DisableAntiforgery()
            .RequireAuthorization("AdminOnlyAccess");

        subtitleGroup.MapPatch("/update", async ([FromBody]UpdateSubtitleCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok();
        }).RequireAuthorization("AdminOnlyAccess");

        subtitleGroup.MapDelete("/delete", async ([FromBody] DeleteSubtitleCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.NoContent();
        }).RequireAuthorization("AdminOnlyAccess");
        #endregion

        return app;
    }
}
