using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.dbContext;
using ServiceLayer.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;

namespace APILayer.Extension
{
    public static class ServiceExtension
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            // Register all services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService,ProductService>();
            // Add more services here
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            // Add more repositories here
        }

        public static void ConfigurePostgresDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}
