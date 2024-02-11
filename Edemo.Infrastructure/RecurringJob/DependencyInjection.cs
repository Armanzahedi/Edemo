using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edemo.Infrastructure.RecurringJob;

public static class DependencyInjection
{
    public static IServiceCollection AddRecurringJobs(this IServiceCollection services,IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
                services.AddHangfire(x => x.UseMemoryStorage());
            else
                services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();

            var recurringJobServices = GetRecurringJobs();

            foreach (var item in recurringJobServices)
            {
                services.AddScoped(item);
            }

            return services;
        }

        public static void StartRecurringJobs(this WebApplication app)
        {
            app.UseHangfireDashboard();

            using var scope = app.Services.CreateScope();
            
            var recurringJobServices = GetRecurringJobs();
            
            foreach (var item in recurringJobServices)
            {
                var attribute =
                    (CronScheduleAttribute)item.GetCustomAttributes(typeof(CronScheduleAttribute), false)[0];
                
                var cronExpression = attribute.CronExpression;
                if (string.IsNullOrEmpty(cronExpression))
                    throw new Exception($"Recurring job {item} doesn't have a cron expression");


                var recurringJobService = (IRecurringJob)scope.ServiceProvider.GetRequiredService(item);
                Hangfire.RecurringJob.AddOrUpdate(item.Name, () => recurringJobService.Run(), cronExpression);
            }
        }

        private static List<Type> GetRecurringJobs()
        {
            Assembly assembly = typeof(IRecurringJob).Assembly;
            Type interfaceType = typeof(IRecurringJob);

            var recurringJobServices = assembly.GetExportedTypes()
                .Where(t => t.IsClass && interfaceType.IsAssignableFrom(t))
                .ToList();
            return recurringJobServices;
        }
    }
