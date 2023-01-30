using CommonLibrary.Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace CommonLibrary.AspNetCore.Identity.Policies;

public static class UserPolicyFactory
{
    public static IPolicy GetPolicy()
    {
        return new UserPolicy();    
    }
  
}
public class UserPolicy : IPolicy
{
    public const string ELEVATED_RIGHTS = "UserPolicies_ElevatedRights";
    public const string AUTHENTICATED = "UserPolicies_Authenticated";

    public AuthorizationOptions Enforce(AuthorizationOptions options)
    {
        options.AddPolicy(ELEVATED_RIGHTS, 
            policy =>
            policy.RequireClaim(
                UserClaimTypes.Role,
                "Admin"));
        options.AddPolicy(AUTHENTICATED, policy =>
            policy.RequireAuthenticatedUser());
        return options;    
    }
    
}