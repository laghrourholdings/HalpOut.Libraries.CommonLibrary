namespace CommonLibrary.AspNetCore.Identity;

public class RolePrincipal
{
    public class UserPermission
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
    }

    public List<string> Roles { get; set; } = new();
    public List<UserPermission> Permissions { get; set; } = new();
}