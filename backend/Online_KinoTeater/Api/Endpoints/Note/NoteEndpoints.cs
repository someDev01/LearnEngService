using Api.Requests;
using Application.Common.Claims;
using Application.Note.Commands.CreateNote;
using Application.Note.Commands.DeleteNote;
using Application.Note.Commands.UpdateNote;
using Application.Note.Commands.UpdateRepetition;
using Application.Note.Queries.GetAllNotes;
using Application.Note.Queries.GetPagedNotes;
using Application.Note.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Endpoints.Note;

public static class NoteEndpoints
{
    public static IEndpointRouteBuilder AddNoteEndpoints(this IEndpointRouteBuilder app)
    {
        var noteGroup = app.MapGroup("note")
            .WithTags("Notes");

        var filterGroup = app.MapGroup("notes")
            .WithTags("Search");

        #region ENDPOINTS NOTES
        noteGroup.MapGet("/dictionary", async (ClaimsPrincipal claims, IMediator mediator) =>
        {
            var userId = claims.GetUserId();

            var result = await mediator.Send(new GetDictionaryQuery(userId));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        noteGroup.MapGet("/all", async (
            ClaimsPrincipal claims,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new GetPagedNotesQuery(
                userId,
                page,
                pageSize));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);

        }).RequireAuthorization();

        noteGroup.MapPost("/create", async (
            [FromBody] CreateNoteRequest request,
            ClaimsPrincipal claims,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new CreateNoteCommand(
                userId,
                request.Word,
                request.Translations,
                request.Examples!));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        noteGroup.MapPost("/create/with/context", async (
            [FromBody] CreateNoteWithContextRequest request,
            ClaimsPrincipal claims,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new CreateNoteWithContextCommand(
                userId,
                request.YoutubeVideoId,
                request.YoutubeId,
                request.YoutubeVideoTitle,
                new Application.SharedDtos.DurationContextDto(
                    request.Hours,
                    request.Minutes,
                    request.Seconds),
                request.Word,
                request.Context));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        noteGroup.MapPatch("/update", async (
            [FromBody] UpdateNoteRequest request,
            ClaimsPrincipal claims, 
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new UpdateNoteCommand(
                userId,
                request.NoteId,
                request.Word,
                request.Translations,
                request.Examples));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        noteGroup.MapPatch("/updateRepetitionScore", async(
            [FromBody]UpdateRepetitionScoreRequest request,
            ClaimsPrincipal claims,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new UpdateRepetitionScoreCommand(
                userId,
                request.NoteId,
                request.IsCorrect));

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);
            return Results.Ok();
        }).RequireAuthorization();

        noteGroup.MapDelete("/delete", async (
            [FromQuery]Guid noteId, 
            ClaimsPrincipal claims,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();
            var result = await mediator.Send(new DeleteNoteCommand(
                userId,
                noteId));
            if(!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok();
        }).RequireAuthorization();
        #endregion

        #region ENPOINT SEARCH
        filterGroup.MapGet("/search", async (
            ClaimsPrincipal claims,
            [FromQuery] string query,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IMediator mediator) =>
        {
            var userId = claims.GetUserId();

            var result = await mediator.Send(new SearchNotesQuery(
                userId,
                query,
                page,
                pageSize));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);

        }).RequireAuthorization();
        #endregion

        return app;
    }
}
