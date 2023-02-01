using System.Reflection;
using CommonLibrary.ClientServices.Identity;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ClientServices.Core.Extentions;

public static class StartupExtensions
{
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services, bool withSecuroman = true)
    {
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
        services.AddSingleton<ISecuroman, Securoman>();
        return services;
    }
}