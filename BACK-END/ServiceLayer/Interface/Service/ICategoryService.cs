using ServiceLayer.Product.Dto;

namespace ServiceLayer.Interface
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAllCategories();
        CategoryDto? GetCategoryById(Guid id);
        Guid CreateCategory(CreateCategoryDto dto);
        bool UpdateCategory(Guid id, UpdateCategoryDto dto);
        bool DeleteCategory(Guid id);
        List<CategoryHierarchyDto> GetCategoryHierarchy();
    }
}
