using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Commands.UpdateYoutubeVideo;

public record UpdateYoutubeVideoCommand(
    Guid Id,
    string? Title,
    string? LexicalComplexity) : IRequest<Result>;
