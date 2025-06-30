using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceLayer.Models;
using System.Text.RegularExpressions;

namespace ServiceLayer.dbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Role> role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        // Helper to convert PascalCase → snake_case
        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Handle acronyms and consecutive uppercase letters
            var result = Regex.Replace(
                input,
                @"([a-z0-9])([A-Z])|([A-Z])([A-Z][a-z])",
                m => m.Groups[1].Success ? $"{m.Groups[1].Value}_{m.Groups[2].Value}" : $"{m.Groups[3].Value}_{m.Groups[4].Value}",
                RegexOptions.Compiled
            ).ToLower();

            return result;
        }
    }
}