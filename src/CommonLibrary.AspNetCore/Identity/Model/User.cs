using CommonLibrary.Logging;
using Microsoft.AspNetCore.Identity;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class User : IdentityUser, ILoggable
{
    public Guid LogHandleId { get; set; }
    public Guid UserDetailsId { get; set; }
    public Guid UserDeviceId { get; set; }
    public Guid UserInterestId { get; set; }
    public Guid SessionId { get; set; }
    
}