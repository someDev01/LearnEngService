namespace Application.Interfaces.Token;

public interface ITokenService
{
    string GenerationToken(Domain.Model.Entyties.User user);
}
