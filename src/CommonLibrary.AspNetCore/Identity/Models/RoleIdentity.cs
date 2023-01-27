namespace CommonLibrary.AspNetCore.Identity;

public class RoleIdentity
{
    public string Name { get; set; }
    public List<UserPermission> Permissions { get; set; } = new();
}