using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class UserSession : IObject, ISuspendable
{
    public Guid Id { get; set; }
    public UserDevice DeviceId { get; set; }
    public string Key { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public string? Descriptor { get; set; }
    public byte[] RawAuthenticationTicket { get; set; }

    public bool IsSuspended { get; set; } = false;
    public DateTimeOffset SuspendedDate { get; set; }
    public Guid SuspendedBy { get; set; } = Guid.Empty;
}