using System.Reflection;
using CommonLibrary.ClientServices.Identity;
using CommonLibrary.ClientServices.Identity.Handler;
using CommonLibrary.ClientServices.Policies;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ClientServices.Core.Extentions;

public static class StartupExtensions
{
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services, bool withSecuroman = true)
    {
        //FlurlHttp.Configure(x=>x.)
        services.AddTransient<SecuromanCookieHandler>();
        services.AddScoped(sp => sp
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"))
            .AddHttpClient("HttpClient", client => client.BaseAddress = new Uri(Api.UserService))
            .AddHttpMessageHandler<SecuromanCookieHandler>()
            .AddPolicyHandler(request => new HttpClientPolicy().LinearHttpRetryPolicy);
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetEntryAssembly());
#if DEBUG
            options.UseReduxDevTools();
#endif
        });
        services.AddOptions();
        if (!withSecuroman) return services;
        
        services.AddAuthorizationCore();
        services.AddScoped<ICookie, Cookie>();
        services.AddScoped<ISecuroman, Securoman>();
        return services;
    }
}