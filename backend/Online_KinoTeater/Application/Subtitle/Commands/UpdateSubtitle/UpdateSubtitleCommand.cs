using Domain.Model.Common;
using MediatR;

namespace Application.Subtitle.Commands.UpdateSubtitle;

public record UpdateSubtitleCommand(
    Guid SubtitleId,
    string? File,
    string? Format): IRequest<Result>;
