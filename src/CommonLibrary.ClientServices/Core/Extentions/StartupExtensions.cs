using System.Reflection;
using Blazored.LocalStorage;
using CommonLibrary.ClientServices.Core.LocalStorage;
using CommonLibrary.ClientServices.Identity;
using CommonLibrary.ClientServices.Policies;
using Fluxor;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ClientServices.Core;

public static class StartupExtensions
{
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services, bool withSecuroman = true)
    {
        //FlurlHttp.Configure(x=>x.)
        services.AddBlazoredLocalStorage();
        services.AddTransient<SecuromanCookieHandler>();
        services.AddScoped(sp => sp
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"))
            .AddHttpClient("HttpClient", client => client.BaseAddress = new Uri(Api.UserService))
            .AddHttpMessageHandler<SecuromanCookieHandler>()
            .AddPolicyHandler(request => new HttpClientPolicy().LinearHttpRetryPolicy);
        services.AddScoped<IStringStateStorage, LocalStateStorage>();
        services.AddScoped<IStoreHandler, JsonStoreHandler>();
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetEntryAssembly());
#if DEBUG
            options.UseReduxDevTools();
#endif
            options.UsePersist();
        });
        services.AddOptions();
        services.AddScoped<ICookies, Cookies>();
        if (!withSecuroman) return services;
        
        services.AddAuthorizationCore();
        services.AddScoped<ISecuroman, Securoman>();
        return services;
    }
}