using CommonLibrary.Core;
using CommonLibrary.Identity.Models.Dtos;

namespace CommonLibrary.ClientServices.Identity;

public class User : IUser
{
    public Guid Id {  get;  set; }
    public Guid LogHandleId {  get;  set; }
    public List<UserClaim> UserClaims {  get;  set; }
}