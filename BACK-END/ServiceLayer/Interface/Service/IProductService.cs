using ServiceLayer.Product.Dto;

namespace ServiceLayer.Interface
{
    public interface IProductService
    {
        List<CategoryDto> GetAllCategories();
        List<ProductDto> GetAllProducts();
    }
}
