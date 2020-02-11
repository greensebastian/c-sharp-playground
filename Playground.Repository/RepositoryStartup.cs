using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Repository.Data;

namespace Playground.Repository
{
    public static class RepositoryStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PlaygroundDatabaseContext>(builder =>
            {
                builder.UseLazyLoadingProxies();
                builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly("Playground"));
            });
            services.AddTransient<PlaygroundDatabaseContext>();
            services.AddTransient<TimelineRepository>();
        }
    }
}
