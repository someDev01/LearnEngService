using Application.Translate.Commands.Translate;
using MediatR;

namespace Api.Endpoints.Translate;

public static class TranslateEndpoints
{
    public static IEndpointRouteBuilder AddTranslateEndpoints(this IEndpointRouteBuilder app)
    {
        var translateGroup = app.MapGroup("api/translate")
            .WithTags("Show Translation");

        translateGroup.MapGet("/show/translation", async (string word, IMediator mediator) =>
        {
            var result = await mediator.Send(new TranslateWordCommand(word));
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error!);

            return Results.Ok(result.Value);
        }).RequireAuthorization();

        return app;
    }
}
