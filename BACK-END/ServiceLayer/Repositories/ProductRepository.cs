using Microsoft.EntityFrameworkCore;
using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;
using System.Linq;

namespace ServiceLayer.Repository
{
    public class ProductRepository : RepositoryBase<Models.Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Models.Product> GetAllProducts()
        {
            return FindByCondition(product => product.IsActive); // Assuming you have an IsActive field
        }

        public void AddProductWithDetails(Models.Product product, List<ProductVariant> variants, List<ProductImage> images)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Add main product
                _context.Products.Add(product);

                // Add product variants
                if (variants != null && variants.Any())
                {
                    _context.ProductVariants.AddRange(variants);
                }

                // Add product images
                if (images != null && images.Any())
                {
                    _context.ProductImages.AddRange(images);
                }

                // Save all changes
                _context.SaveChanges();

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback on failure
                transaction.Rollback();

                // Optional: Log or rethrow
                throw new Exception("Failed to add product with details", ex);
            }
        }

        public List<ProductVariant> GetVariantsByProductId(Guid productId)
        {
            return _context.ProductVariants
                .Where(v => v.ProductId == productId)
                .ToList();
        }

        public List<ProductImage> GetImagesByProductId(Guid productId)
        {
            return _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.DisplayOrder) // Optional: To show primary/ordered images first
                .ToList();
        }


        public List<Review> GetReviewsByProductId(Guid productId)
        {
            return _context.Reviews
        .Where(r => r.ProductId == productId && r.IsApproved)
        .OrderByDescending(r => r.CreatedAt)
        .ToList();
        }

        public Review? GetReviewById(Guid reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public void CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public bool DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            return _context.SaveChanges() > 0;
        }

        public bool HasPurchased(Guid userId, Guid productId)
        {
            return _context.OrderItems
                .Include(oi => oi.Variant)
                .Any(oi =>
                    oi.Order.UserId == userId &&
                    oi.Variant.ProductId == productId &&
                    oi.Order.Status == OrderStatus.Delivered
                );

        }
        public Discount? GetActiveDiscountByCode(string code)
        {
            return _context.Discounts
                .FirstOrDefault(d =>
                    d.Code == code &&
                    d.IsActive &&
                    d.StartDate <= DateTime.UtcNow &&
                    d.EndDate >= DateTime.UtcNow);
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }


    }
}
