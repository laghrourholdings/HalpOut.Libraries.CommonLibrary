using System.Security.Claims;
using CommonLibrary.AspNetCore.Logging;
using CommonLibrary.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.AspNetCore.Identity;


public class SecuromanMiddleware
{
    private readonly RequestDelegate requestDelegate;
       
    public SecuromanMiddleware(RequestDelegate requestDelegate)
    {
        this.requestDelegate = requestDelegate;
          
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILoggingService loggingService,
        ISecuromanService securomanService)
    {
        try
        {
            if (context.Request.Cookies.TryGetValue("Identity.Session", out var session)
                && context.Request.Cookies.TryGetValue("Identity.Token", out var token))
            {
                if (token != null)
                {
                    var verify = await securomanService.Identify(token);
                    if (verify != null && verify.Result.IsValid)
                    {
                        context.User.AddIdentity(new ClaimsIdentity(verify.Claims.Select(x => new Claim(x.Type, x.Value)), "Securoman"));
                    }
                    else
                    {
                        var payload = Securoman.GetUnverifiedUserTicket(token);
                        var loghandleId = payload?.FirstOrDefault(x=>x.Type == UserClaimTypes.LogHandleId)?.Value; 
                        var userId = payload?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
                        if (loghandleId != null && userId != null)
                        {
                            loggingService.Information($"Authentification rejected for user [invalid token]", new Guid(loghandleId));
                        }
                    }
                }
            }
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            loggingService.Local().Error("Authentification error in SecuromanMiddleware: {Ex}", ex);
        }
    }
}