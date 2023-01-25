using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonLibrary.AspNetCore.Identity;

public class SecuromanAuthenticationHandler : AuthenticationHandler<SecuromanAuthenticationOptions>
    {
        private readonly ISecuromanService _securomanService;
        private readonly IConfiguration _config;
        public const  string SchemaName = "Securoman";

        public SecuromanAuthenticationHandler(
            IOptionsMonitor<SecuromanAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISecuromanService securomanService,
            IConfiguration config)
            : base(options, logger, encoder, clock)
        {
            _securomanService = securomanService;
            _config = config;
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
                    var refreshedToken = await Securoman.GetSecuromanUrl(_config)
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