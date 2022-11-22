using Microsoft.AspNetCore.Authorization;

namespace CommonLibrary.AspNetCore.Identity;

public static class Policies
{
    public static readonly string ELEVATED_RIGHTS = "ElevatedRights";
    public static readonly string AUTHENTICATED = "Authenticated";
    public static AuthorizationOptions UserPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(ELEVATED_RIGHTS, policy =>
            policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator"));
        options.AddPolicy(AUTHENTICATED, policy =>
            policy.RequireAuthenticatedUser());
        return options;
    }
}