using Microsoft.AspNetCore.Identity;

namespace CommonLibrary.AspNetCore.Identity;

public class UserBadge
{
    public Guid UserId { get; set; }
    public Guid LogHandleId { get; set; }
    public byte[] SecretKey { get; set; }
    
    public RolePrincipal RolePrincipal { get; set; }
}