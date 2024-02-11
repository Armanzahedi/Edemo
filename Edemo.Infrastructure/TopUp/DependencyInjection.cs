using Edemo.Domain.TopUp;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Edemo.Infrastructure.TopUp;

public static class DependencyInjection
{
    public static IServiceCollection AddTopUpServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TopUpOptions>(configuration.GetSection(TopUpOptions.TopUp));
        services.AddScoped<ITopUpOptions>(sp => sp.GetRequiredService<IOptions<TopUpOptions>>().Value);
        
        return services;
    }
}