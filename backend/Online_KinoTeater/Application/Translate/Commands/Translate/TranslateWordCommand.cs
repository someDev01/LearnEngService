using Domain.Model.Common;
using MediatR;

namespace Application.Translate.Commands.Translate;

public record TranslateWordCommand(string Word): IRequest<Result<string>>;
