using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using CommonLibrary.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonLibrary.AspNetCore.Identity;

public class SecuromanAuthenticationHandler : AuthenticationHandler<SecuromanAuthenticationOptions>
    {
        private readonly ISecuromanService _securomanService;
        public const  string SchemaName = "Securoman";

        public SecuromanAuthenticationHandler(
            IOptionsMonitor<SecuromanAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISecuromanService securomanService)
            : base(options, logger, encoder, clock)
        {
            _securomanService = securomanService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey(SecuromanDefaults.TokenCookie) || 
                !Request.Cookies.ContainsKey(SecuromanDefaults.SessionCookie))
            {
                return AuthenticateResult.Fail("Unauthorized - missing required metadata.");
            }
            // get the value of the authorization header
            string? token = Request.Cookies[SecuromanDefaults.TokenCookie];
            string? session = Request.Cookies[SecuromanDefaults.SessionCookie];
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(session))
            {
                return AuthenticateResult.NoResult();
            }
            try
            {
                var result = await _securomanService.Authenticate(token);
                if (!result.Succeeded) 
                    return AuthenticateResult.Fail(result.ErrorMessage);
                var identity = new ClaimsIdentity(result.Claims, SchemaName);
                var principal = new GenericPrincipal(identity, result.RolePrincipal.Roles.ToArray());
                var ticket = new AuthenticationTicket(principal, SchemaName);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
    }

    public class SecuromanAuthenticationOptions : AuthenticationSchemeOptions
    { }