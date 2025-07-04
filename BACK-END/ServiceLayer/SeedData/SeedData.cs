using Microsoft.EntityFrameworkCore;
using ServiceLayer.Models;
using ServiceLayer.dbContext;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLayer.DbSeeder
{
    public static class SeedData
    {

        private static AppDbContext context;
       
         public static void SeedDatabase(IServiceProvider services)
        {
           

            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

         //   context.Database.Migrate();

            SeedRoles(context);
            SeedCategoriesFromCsv(context);
        }

        private static void SeedRoles(AppDbContext context)
        {
            if (!context.role.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "Customer" },
                    new Role { Id = 3, Name = "Seller" }
                };
                context.role.AddRange(roles);
                context.SaveChanges();
            }
        }

        private static void SeedCategoriesFromCsv(AppDbContext context)
        {
            if (context.Categories.Any()) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "categories.csv");
            if (!File.Exists(filePath)) return;

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CategoryCsv>().ToList();

            var categories = records.Select(r => new Category
            {
                Id = Guid.NewGuid(),
                Name = r.Name,
                Slug = r.Slug,
                CreatedAt = DateTime.UtcNow
            });

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private class CategoryCsv
        {
            public string Name { get; set; } = null!;
            public string Slug { get; set; } = null!;
        }
    }
}
