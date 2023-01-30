using CommonLibrary.Core;
using CommonLibrary.Identity.Models.Dtos;

namespace CommonLibrary.ClientServices.Identity.Models;

public class User : IUser
{
    public bool IsAuthenticated {  get;  set; }
    public Guid Id {  get;  set; }
    public Guid LogHandleId {  get;  set; }
    public List<UserClaim> UserClaims {  get;  set; }
}