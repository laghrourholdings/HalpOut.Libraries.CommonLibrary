using System.Security.Claims;
using CommonLibrary.AspNetCore.Logging;
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
            var authenticationResult = await securomanService.Authenticate();
            // Do something with AuthenticationResult .. 
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            loggingService.Local().Error("Authentification error in SecuromanMiddleware: {Ex}", ex);
        }
    }
}