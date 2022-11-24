using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class UserSession : IObject
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? DeviceId { get; set; }
    public string Key { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public string? Descriptor { get; set; }
    public byte[] RawAuthenticationTicket { get; set; }
}