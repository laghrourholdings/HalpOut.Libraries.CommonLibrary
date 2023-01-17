using System.Reflection;
using CommonLibrary.AspNetCore.Core.Configurations;
using CommonLibrary.AspNetCore.Core.Policies;
using CommonLibrary.AspNetCore.Identity;
using CommonLibrary.AspNetCore.Logging;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CommonLibrary.AspNetCore.Core;
/*[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly ClientPolicy _clientPolicy;
    private readonly IHttpClientFactory _clientFactory;

    public RequestController(ClientPolicy clientPolicy, IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
        _clientPolicy = clientPolicy;
    }
    
    [HttpGet]
    public async Task<ActionResult> MakeRequest()
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7111/response/30");
        //var response = await _clientPolicy.LinearHttpRetryPolicy.ExecuteAsync(
        //    ()=>client.GetAsync("https://localhost:7111/response/30"));
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("--> Request successful");
            return Ok();
        }
        Console.WriteLine("--> Request {un}successful");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}*/
public static class StartupExtentions
{
    
    /// <summary>
    /// Main bootstrap method for the components provided by the CommonLibrary.AspNetCore
    /// Every back-end servince running on Asp.Net must call this method in program.cs
    /// </summary>
    /// <param name="configuration">IConfiguration provided by builder.Configuration</param>
    /// <param name="logging">ILoggingBuilder used in conjunction with the LoggerConfiguration</param>
    /// <param name="loggerConfiguration">ILoggerConfiguration binded with Serilog's ILogger</param>
    /// <param name="originName">Name of the CORS origin policy (defaulted to none)</param>
    public static IServiceCollection AddCommonLibrary(this IServiceCollection services,
        IConfiguration configuration, ILoggingBuilder logging, LoggerConfiguration loggerConfiguration , string originName)
    {
        services.AddHttpContextAccessor();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        logging.ClearProviders();
        services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
        services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        RabbitMQSettings rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>() ?? throw new InvalidOperationException("RabbitMQSettings is null");
        ILogger logger = loggerConfiguration
            .Enrich.WithEnvironmentName().WriteTo.ServiceBusSink().Enrich.WithProperty("servicename", serviceSettings.ServiceName)
#if RELEASE
            .MinimumLevel.Warning()      
#endif
            .CreateLogger();
        
        logging.AddSerilog(logger);
        services.AddSingleton(logger);
        services.AddCors(options =>
        {
            options.AddPolicy(name: originName,
                policy  =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
        });
        services.AddHttpClient("HttpClient").AddPolicyHandler(
            request => new HttpClientPolicy().LinearHttpRetryPolicy);
        services.AddSingleton<HttpClientPolicy>();
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
        services.AddCommonLibraryRabbitMq();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static IServiceCollection AddCommonLibraryLoggingService(this IServiceCollection services)
    {
        services.AddScoped<ILoggingService, LoggingService>();
        return services;
    }
    public static IServiceCollection AddCommonLibrarySecuroman(this IServiceCollection services) 
    {
        services.AddSingleton<ISecuromanService, SecuromanService>();
        return services;
    }
    public static IServiceCollection AddCommonLibraryRabbitMq(this IServiceCollection services, params IConsumer[] additionalConsumers)
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumers(Assembly.GetEntryAssembly());
            foreach (var consumerType in additionalConsumers)
            {
                config.AddConsumer(consumerType.GetType());
            }
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
    public static WebApplication UseCommonLibrary(this WebApplication app, string originName)
    {
        app.UseHttpsRedirection();
        app.UseCors(originName);
        app.MapControllers();
        return app;
    }
    public static WebApplication UseCommonLibrarySecuroman(this WebApplication app)
    {
        app.UseMiddleware<SecuromanMiddleware>();
        return app;
    }
}
