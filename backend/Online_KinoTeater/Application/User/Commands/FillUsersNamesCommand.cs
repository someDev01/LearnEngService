using Domain.Model.Common;
using MediatR;

namespace Application.User.Commands;

public record FillUsersNamesCommand(): IRequest<Result>;