using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class UserSession : IObject, IDeletable
{
    public Guid Id { get; set; }
    public UserDevice Device { get; set; }
    public string Key { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public string? Descriptor { get; set; }
    public byte[] RawAuthenticationTicket { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset DeletedDate { get; set; }
    public Guid DeletedBy { get; set; } = Guid.Empty;
}