using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using ServiceLayer.Interface.Repository;
using ServiceLayer.Product.Dto;
using ServiceLayer.Models;
using Shared.Services.Exceptions;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public List<CategoryDto> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories.");

            var categories = _productRepository.GetAllCategories().ToList();

            if (!categories.Any())
            {
                _logger.LogWarning("No categories found.");
                throw new NotFoundCustomException("No categories found.");
            }

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentId = c.ParentId,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public List<ProductDto> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");

            var products = _productRepository.GetAllProductsWithCategory(); // Includes Category

            if (!products.Any())
            {
                _logger.LogWarning("No products found.");
                throw new NotFoundCustomException("No products found.");
            }

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CategoryName = p.Category?.Name // safe null-check
            }).ToList();
        }
    }
}
