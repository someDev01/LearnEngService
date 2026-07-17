namespace Application.User.Dtos;

public record UserDto(
    Guid Id,
    string Email, 
    string Name,
    string Role, 
    DateTime CreatedAt);
