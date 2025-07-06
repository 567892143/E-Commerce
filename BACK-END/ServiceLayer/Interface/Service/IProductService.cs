using ServiceLayer.Product.Discount.Dto;
using ServiceLayer.Product.Dto;
using ServiceLayer.ProductDetail.Dto;
using ServiceLayer.UpsertProduct.Dto;

namespace ServiceLayer.Interface
{
    public interface IProductService
    {
        List<CategoryDto> GetAllCategories();
        List<ProductDto> GetAllProducts();

        Task<ProductDetailDto?> GetProductById(Guid id);
        List<ProductDto> GetProductsByCategory(Guid categoryId);
        List<VariantDto> GetVariantsByProductId(Guid productId);
        List<ImageDto> GetImagesByProductId(Guid productId);
        Guid CreateProduct(CreateProductDto dto);
        bool UpdateProduct(Guid id, UpdateProductDto dto);
        Task<bool> DeleteProduct(Guid id);

        List<ReviewDto> GetReviewsByProductId(Guid productId);
        void AddReview(Guid userId, Guid productId, CreateReviewDto dto);
        bool DeleteReview(Guid userId, Guid reviewId);


        DiscountDto? ValidateDiscount(string code);
        void CreateDiscount(CreateDiscountDto dto);
        DiscountResultDto? ApplyDiscount(ApplyDiscountDto dto);

    }
}
