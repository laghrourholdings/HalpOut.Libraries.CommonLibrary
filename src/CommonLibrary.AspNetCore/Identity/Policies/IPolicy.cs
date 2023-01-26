using Microsoft.AspNetCore.Authorization;

namespace CommonLibrary.AspNetCore.Identity.Policies;

public interface IPolicy
{
    public AuthorizationOptions Enforce(AuthorizationOptions options);
}