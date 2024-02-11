using System.Reflection;
using Edemo.Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edemo.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        AddDomainServices(services);
        return services;
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IDomainService).IsAssignableFrom(type))
            .ToList();

        foreach (var type in typesToRegister)
        {
            services.AddScoped(type);
        }
    }
}