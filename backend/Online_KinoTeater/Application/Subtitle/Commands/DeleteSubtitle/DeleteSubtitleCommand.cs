using Domain.Model.Common;
using MediatR;

namespace Application.Subtitle.Commands.DeleteSubtitle;

public record DeleteSubtitleCommand(
    Guid SubtitleId,
    string FileKey): IRequest<Result>;
