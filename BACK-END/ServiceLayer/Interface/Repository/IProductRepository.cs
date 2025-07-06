
using ServiceLayer.User.Dto;
using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;
using ServiceLayer.Shared.Services;
namespace ServiceLayer.Interface.Reposiory;

public interface IProductRepository : IRepositoryBase<ServiceLayer.Models.Product>
{
    IQueryable<Models.Product> GetAllProducts();

    void AddProductWithDetails(Models.Product product, List<ProductVariant> variants, List<ProductImage> images);

    List<ProductVariant> GetVariantsByProductId(Guid productId);
    List<ProductImage> GetImagesByProductId(Guid productId);


    // 👇 Review-related methods
    List<Review> GetReviewsByProductId(Guid productId);
    Review? GetReviewById(Guid reviewId);
    void CreateReview(Review review);
    bool DeleteReview(Review review);
    bool HasPurchased(Guid userId, Guid productId);


    Discount? GetActiveDiscountByCode(string code);
    void AddDiscount(Discount discount);


}