using Edemo.Domain.Common;
using Edemo.Infrastructure.Persistence.Audit.Interceptors;
using Edemo.Infrastructure.Persistence.Intreceptors;
using Edemo.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edemo.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
    {
        
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteSaveChangeInterceptor>();
        
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("AppDb"));
            
            services.AddScoped<IUnitOfWork, UnitOfWorkInMemory>();

        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
        
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        
        services.AddScoped<AppDbContextInitializer>();
        
        return services;
    }
}