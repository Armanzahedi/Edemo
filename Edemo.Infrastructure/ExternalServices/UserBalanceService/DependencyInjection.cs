using Edemo.Domain.ExternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Edemo.Infrastructure.ExternalServices.UserBalanceService;

public static class DependencyInjection
{
    public static IServiceCollection AddUserBalanceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UserBalanceServiceOptions>(configuration.GetSection(UserBalanceServiceOptions.UserBalanceService));
        
        services
            .AddRefitClient<IUserBalanceService>()
            .ConfigureHttpClient((sp, c) => c.BaseAddress = new Uri(sp.GetRequiredService<IOptions<UserBalanceServiceOptions>>().Value.Address));
        
        return services;
    }
}