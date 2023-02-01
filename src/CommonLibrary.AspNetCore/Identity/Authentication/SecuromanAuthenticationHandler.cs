using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonLibrary.AspNetCore.Identity.Authentication;

public class SecuromanAuthenticationHandler : AuthenticationHandler<SecuromanAuthenticationOptions>
    {
        private readonly ISecuroman _securoman;
        public const  string SchemaName = "Securoman";

        public SecuromanAuthenticationHandler(
            IOptionsMonitor<SecuromanAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISecuroman securoman)
            : base(options, logger, encoder, clock)
        {
            _securoman = securoman;
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
                var result = await _securoman.Authenticate(token);
                if (!result.Succeeded)
                {
                    /*if(!result.RefreshToken)
                        return AuthenticateResult.Fail(result.ErrorMessage);*/
                    var refreshedToken = await _securoman.GetSecuromanUrl()
                        .WithHeader("User-Agent", Request.Headers.UserAgent)
                        .WithCookies(Request.Cookies)
                        .AppendPathSegment("api/v1/user")
                        .AppendPathSegment("refreshToken")
                        .GetStringAsync();
                    if(refreshedToken==null)
                        return AuthenticateResult.Fail("Unauthorized - invalid token");
                    var secondResult = await _securoman.Authenticate(refreshedToken);
                    if(!secondResult.Succeeded) 
                        return AuthenticateResult.Fail(secondResult.ErrorMessage);
                    result = secondResult;
                    Response.Cookies.Append(SecuromanDefaults.TokenCookie, refreshedToken, 
                        new CookieOptions
                    {
                        Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0)),
                        Secure = true
                    });
                }
                var identity = new ClaimsIdentity(result.Claims, SchemaName);
                //TODO: urgent, fix this: Authorize attribute should not look at roles set here
                var principal = new GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, SchemaName);
                // Context.Items.Add(SecuromanDefaults.ContextPermissions, result.RolePrincipal.SelectMany(x=>x.Properties)
                //     .Where(x=>x.Type==UserClaimTypes.Right || x.Type == UserClaimTypes.Privilege).Select(x=>x.Value).ToList());
                // Context.Items.Add(SecuromanDefaults.ContextRoles, result.RolePrincipal.SelectMany(x=>x.Name).ToList());
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