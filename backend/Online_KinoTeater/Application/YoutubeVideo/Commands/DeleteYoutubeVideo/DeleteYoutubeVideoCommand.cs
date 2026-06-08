using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Commands.DeleteYoutubeVideo;

public record DeleteYoutubeVideoCommand(
    Guid YoutubeVideoId): IRequest<Result<Guid>>;
