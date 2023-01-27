namespace CommonLibrary.AspNetCore.Identity;

public class UserBadge
{
    public Guid UserId { get; set; }
    public Guid LogHandleId { get; set; }
    public byte[] SecretKey { get; set; }

    public List<RoleIdentity> RolePrincipal { get; set; } = new();
}