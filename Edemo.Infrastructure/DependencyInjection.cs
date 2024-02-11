using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Edemo.Domain.Common;
using Edemo.Infrastructure.Cache;
using Edemo.Infrastructure.Identity;
using Edemo.Infrastructure.Persistence;
using Edemo.Infrastructure.Common;
using Edemo.Infrastructure.ExternalServices.UserBalanceService;
using Edemo.Infrastructure.TopUp;

namespace Edemo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
          
        // services.AddRecurringJobs(configuration);
        services.AddUserBalanceService(configuration);
        services.AddPersistence(configuration);
        services.AddIdentity(configuration);
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTopUpServices(configuration);
        services.AddCacheService(configuration);
        
        return services;
    }
}