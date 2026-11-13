using Domain.Model.Common;
using MediatR;

namespace Application.Subtitle.Commands.CreateSubtitle;

public record CreateSubtitleCommand(
    Guid VideoId,
    string LanguageCode,
    Stream FileStream,
    string Key,
    string Format,
    string ContentType): IRequest<Result<Guid>>;
