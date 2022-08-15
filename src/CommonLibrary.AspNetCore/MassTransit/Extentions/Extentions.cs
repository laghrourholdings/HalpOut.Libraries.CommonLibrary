using System.Reflection;
using CommonLibrary.AspNetCore.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.AspNetCore.MassTransit;

public static class MassTransitExtention
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumers(Assembly.GetEntryAssembly());
            config.UsingRabbitMq((context, configurator) =>
            {
                var configuration = context.GetService<IConfiguration>();
                ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
                RabbitMQSettings rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>() ?? throw new InvalidOperationException("RabbitMQSettings is null");
                configurator.Host(rabbitMQSettings.Host);
                configurator.ConfigureEndpoints(context,
                    new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                configurator.UseMessageRetry(retryConfigurator =>
                {
                    retryConfigurator.Interval(10, TimeSpan.FromSeconds(3));
                });
            });
        });

        return services;
    }
}
