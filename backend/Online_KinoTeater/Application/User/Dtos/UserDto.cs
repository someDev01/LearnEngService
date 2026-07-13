namespace Application.User.Dtos;

public record UserDto(
    Guid Id,
    string Email, 
    string Role, 
    DateTime CreatedAt);
