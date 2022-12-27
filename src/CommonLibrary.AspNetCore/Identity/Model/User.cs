using CommonLibrary.Logging;
using Microsoft.AspNetCore.Identity;

namespace CommonLibrary.AspNetCore.Identity.Model;

public sealed class User : IdentityUser, ILoggable
{
    public Guid LogHandleId { get; set; }
    public UserInterest UserInterest { get; set; } = new();
    public UserDetail UserDetail { get; set; } = new();
    public ICollection<UserDevice> UserDevices { get; set; } = new HashSet<UserDevice>();
    public ICollection<UserSession> UserSessions { get; set; } = new HashSet<UserSession>();

}