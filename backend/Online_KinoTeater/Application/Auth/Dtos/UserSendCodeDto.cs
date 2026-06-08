namespace Application.Auth.Dtos;

public record UserSendCodeDto(
    string Email, 
    string Code);
