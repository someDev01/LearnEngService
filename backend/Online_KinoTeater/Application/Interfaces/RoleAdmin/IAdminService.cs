namespace Application.Interfaces.RoleAdmin;

public interface IAdminService
{
    Task<bool> ExistsRoleAdmin(CancellationToken cancellationToken);
}
