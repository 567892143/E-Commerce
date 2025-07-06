using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.dbContext;
using ServiceLayer.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Repository;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

namespace APILayer.Extension
{
    public static class ServiceExtension
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            // Register all services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IInventoryService, InventoryService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
        }

        public static void ConfigurePostgresDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}
