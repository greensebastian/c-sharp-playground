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
            services.AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly("Playground"));
            });
            services.AddScoped<TimelineRepository>();
        }
    }
}
