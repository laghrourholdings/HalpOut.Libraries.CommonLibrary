using CommonLibrary.AspNetCore.Logging;
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

    public async Task InvokeAsync(HttpContext context, ILoggingService loggingService)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            loggingService.Local().Error("Error in RefreshJwtMiddleware: {Ex}", ex);
        }
    }
}



public static class SecuromanMiddlewareExtensions
{
    public static IApplicationBuilder UseRefreshJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecuromanMiddleware>();
    }
}