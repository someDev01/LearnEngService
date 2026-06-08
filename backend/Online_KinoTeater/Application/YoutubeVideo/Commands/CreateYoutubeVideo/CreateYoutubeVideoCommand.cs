using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Commands.AddYoutubeVideo;

public record CreateYoutubeVideoCommand(
    string YoutubeVideoUrl,
    string LexicalComplexity): IRequest<Result<Guid>>;