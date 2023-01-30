using System.Reflection;
using CommonLibrary.ClientServices.Identity.AuthService;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ClientServices.Core.Extentions;

public static class StartupExtensions
{
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services)
    {
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetEntryAssembly());
#if DEBUG
            options.UseReduxDevTools();
#endif
        });
        services.AddOptions();
        services.AddAuthorizationCore();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}