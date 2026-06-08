using Application.Auth.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.Auth.Commands.Verify;

public record VerificationCodeCommand(
    string Email,
    string Code) : IRequest<Result<VerifyDto>>;
