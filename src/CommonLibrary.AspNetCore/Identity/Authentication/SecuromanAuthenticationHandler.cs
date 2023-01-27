using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonLibrary.AspNetCore.Identity.Authentication;

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
                {
                    var refreshedToken = await _securomanService.GetSecuromanUrl()
                        .WithHeader("User-Agent", Request.Headers.UserAgent)
                        .WithCookies(Request.Cookies)
                        .AppendPathSegment("api/v1/auth")
                        .AppendPathSegment("refreshToken")
                        .GetStringAsync();
                    if(refreshedToken==null)
                        return AuthenticateResult.Fail("Unauthorized - invalid token");
                    var secondResult = await _securomanService.Authenticate(refreshedToken);
                    if(!secondResult.Succeeded) 
                        return AuthenticateResult.Fail(secondResult.ErrorMessage);
                    result = secondResult;
                    Response.Cookies.Append(SecuromanDefaults.TokenCookie, refreshedToken);
                }
                var identity = new ClaimsIdentity(result.Claims, SchemaName);
                var principal = new GenericPrincipal(identity, result.RolePrincipal.Select(x=>x.Name).ToArray());
                var ticket = new AuthenticationTicket(principal, SchemaName);
                Context.Items.Add(nameof(RoleIdentity), result.RolePrincipal);
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