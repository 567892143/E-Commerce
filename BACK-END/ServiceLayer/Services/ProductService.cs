using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using ServiceLayer.Product.Dto;
using ServiceLayer.Models;
using Shared.Services.Exceptions;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.UpsertProduct.Dto;
using ServiceLayer.ProductDetail.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceLayer.Product.Discount.Dto;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;

        private readonly ICategoryRepository _categoryRepository;


        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public List<CategoryDto> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories.");

            var categories = _productRepository.FindAll().ToList();

            if (!categories.Any())
            {
                _logger.LogWarning("No categories found.");
                throw new NotFoundCustomException("No categories found.");
            }

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public List<ProductDto> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");

            var products = _productRepository.GetAllProducts(); // Includes Category

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
                CategoryName = p.Category.Name ?? " " // safe null-check
            }).ToList();
        }

        public async Task<ProductDetailDto?> GetProductById(Guid id)
        {
            var product = await _productRepository.FindByCondition(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null) return null;

            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                IsFeatured = product.IsFeatured,
                Variants = product.Variants.Select(v => new VariantDto
                {
                    Id = v.Id,
                    Sku = v.Sku,
                    Size = v.Size,
                    Color = v.Color,
                    Price = v.Price,
                    Barcode = v.Barcode,
                    Weight = v.Weight
                }).ToList(),
                Images = product.Images.Select(i => new ImageDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    DisplayOrder = i.DisplayOrder,
                    IsPrimary = i.IsPrimary
                }).ToList()
            };
        }

        public List<ProductDto> GetProductsByCategory(Guid categoryId)
        {
            var products = _productRepository.FindByCondition(p => p.CategoryId == categoryId);
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured
            }).ToList();
        }

        public List<VariantDto> GetVariantsByProductId(Guid productId)
        {
            var variants = _productRepository.GetVariantsByProductId(productId);
            return variants.Select(v => new VariantDto
            {
                Id = v.Id,
                Sku = v.Sku,
                Size = v.Size,
                Color = v.Color,
                Price = v.Price,
                Barcode = v.Barcode,
                Weight = v.Weight
            }).ToList();
        }

        public List<ImageDto> GetImagesByProductId(Guid productId)
        {
            var images = _productRepository.GetImagesByProductId(productId);
            return images.Select(i => new ImageDto
            {
                Id = i.Id,
                Url = i.Url,
                DisplayOrder = i.DisplayOrder,
                IsPrimary = i.IsPrimary
            }).ToList();
        }

        public Guid CreateProduct(CreateProductDto dto)
        {
            var productId = Guid.NewGuid();

            var product = new Models.Product
            {
                Id = productId,
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                CategoryId = dto.CategoryId,
                IsActive = true,
                IsFeatured = dto.IsFeatured,
                CreatedAt = DateTime.UtcNow
            };

            var variants = dto.Variants.Select(v => new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Sku = v.Sku,
                Size = v.Size,
                Color = v.Color,
                Price = v.Price,
                Barcode = v.Barcode,
                Weight = v.Weight,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            var images = dto.Images.Select(i => new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Url = i.Url,
                DisplayOrder = i.DisplayOrder,
                IsPrimary = i.IsPrimary,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _productRepository.AddProductWithDetails(product, variants, images);
            return productId;
        }

        public bool UpdateProduct(Guid id, UpdateProductDto dto)
        {
            var product = new Models.Product
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive,
                IsFeatured = dto.IsFeatured,
                UpdatedAt = DateTime.UtcNow
            };

            _productRepository.Update(product);
            _productRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _productRepository.FindByCondition(p => p.Id == id).FirstOrDefaultAsync();
            _productRepository.Delete(product);
            return true;
        }
        public List<ReviewDto> GetReviewsByProductId(Guid productId)
        {
            var reviews = _productRepository.GetReviewsByProductId(productId);
            var user = _userRepository.GetAllUsers();

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserName = user.Where(u => u.Id == r.UserId).Select(u => u.Name).FirstOrDefault() ?? "anonymus",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                IsVerifiedPurchase = r.IsVerifiedPurchase
            }).ToList();
        }

        public void AddReview(Guid userId, Guid productId, CreateReviewDto dto)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = productId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                IsVerifiedPurchase = _productRepository.HasPurchased(userId, productId),
                IsApproved = true,
                CreatedAt = DateTime.UtcNow
            };

            _productRepository.CreateReview(review);
        }

        public bool DeleteReview(Guid userId, Guid reviewId)
        {
            var review = _productRepository.GetReviewById(reviewId);
            if (review == null || review.UserId != userId) return false;

            return _productRepository.DeleteReview(review);
        }

        public DiscountDto? ValidateDiscount(string code)
        {
            var discount = _productRepository.GetActiveDiscountByCode(code);
            if (discount == null || discount.EndDate < DateTime.UtcNow)
                return null;

            return new DiscountDto
            {
                Code = discount.Code,
                Type = discount.Type,
                Value = discount.Value,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive
            };
        }

        public void CreateDiscount(CreateDiscountDto dto)
        {
            var discount = new Discount
            {
                Id = Guid.NewGuid(),
                Code = dto.Code,
                Name = dto.Name,
                Type = dto.Type,
                Value = dto.Value,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _productRepository.AddDiscount(discount);
        }

        public DiscountResultDto? ApplyDiscount(ApplyDiscountDto dto)
        {
            var discount = _productRepository.GetActiveDiscountByCode(dto.Code);
            if (discount == null) return null;

            decimal discountAmount = 0;

            if (discount.Type == DiscountType.Percentage)
                discountAmount = (dto.OrderTotal * discount.Value) / 100;
            else if (discount.Type == DiscountType.FixedAmount)
                discountAmount = discount.Value;

            var finalAmount = Math.Max(dto.OrderTotal - discountAmount, 0);

            return new DiscountResultDto
            {
                Code = discount.Code,
                Type = discount.Type,
                Value = discount.Value,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount
            };
        }

    }
}
