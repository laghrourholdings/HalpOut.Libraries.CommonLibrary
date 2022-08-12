using CommonLibrary.AspNetCore.MassTransit;
using CommonLibrary.AspNetCore.Policies;
using CommonLibrary.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CommonLibrary.AspNetCore;

public static class StartupExtentions
{
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services,
        IConfiguration configuration, ILoggingBuilder logging, ILogger logger , string originName)
    {
        logging.ClearProviders();

        logging.AddSerilog(logger);
        services.AddSingleton(logger);
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.Configure<ServiceSettings>(configuration.GetSection("ServiceSettings"));
        services.AddCors(options =>
        {
            options.AddPolicy(name: originName,
                policy  =>
                {
                    policy.AllowAnyOrigin();
                });
        });
        services.AddMassTransitWithRabbitMq();
        services.AddHttpClient("HttpClient").AddPolicyHandler(
            request => new HttpClientPolicy().LinearHttpRetryPolicy);
        services.AddSingleton<HttpClientPolicy>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        return services;
    }
    public static WebApplication UseCommonLibrary(this WebApplication app, string originName)
    {
        app.UseCors(originName);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        return app;
    }
}