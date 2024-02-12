using Edemo.Api;
using Edemo.Application;
using Edemo.Domain;
using Edemo.Infrastructure;
using Edemo.Infrastructure.Identity;
using Edemo.Infrastructure.Persistence;
using Edemo.Infrastructure.RecurringJob;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddPresentation()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddDomain(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseIdentityEndpoints("/api/v1.0/auth");

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014


await app.InitializeDb();

app.Run();