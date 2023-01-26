namespace CommonLibrary.AspNetCore.Identity.Roles;

public class RolePrincipal
{
    public List<string> Roles { get; set; } = new();
    public List<UserPermission> Permissions { get; set; } = new();
}