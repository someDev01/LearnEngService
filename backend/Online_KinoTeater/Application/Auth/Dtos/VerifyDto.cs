namespace Application.Auth.Dtos;

public record VerifyDto(
    string Jwt,
    string Email);
