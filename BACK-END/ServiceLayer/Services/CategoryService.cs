using ServiceLayer.Interface;
using ServiceLayer.Product.Dto;
using ServiceLayer.Models;
using ServiceLayer.Interface.Reposiory;
using Microsoft.Extensions.Logging;
using Shared.Services.Exceptions;

namespace ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public List<CategoryDto> GetAllCategories()
        {
            var categories = _categoryRepository.FindAll().ToList();
            if (!categories.Any())
                throw new NotFoundCustomException("No categories found.");

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public CategoryDto? GetCategoryById(Guid id)
        {
            var category = _categoryRepository.FindByCondition(c => c.Id == id).FirstOrDefault();
            if (category == null) return null;


            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<Guid> CreateCategory(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ParentId = dto.ParentId,
                Slug="dummy",
                CreatedAt = DateTime.UtcNow
            };

            _categoryRepository.Create(category);
            await _categoryRepository.SaveAsync();
            return category.Id;
        }

        public bool UpdateCategory(Guid id, UpdateCategoryDto dto)
        {
            var category = _categoryRepository.FindByCondition(c => c.Id == id).FirstOrDefault();
            if (category == null) return false;

            category.Name = dto.Name;
            category.ParentId = dto.ParentId;

            _categoryRepository.Update(category);
            _categoryRepository.SaveAsync();

            return true;
        }

        public bool DeleteCategory(Guid id)
        {
            var category = _categoryRepository.FindByCondition(c => c.Id == id).FirstOrDefault();
            if (category == null) return false;


            _categoryRepository.Delete(category);
            _categoryRepository.SaveAsync();

            return true;
        }

        public List<CategoryHierarchyDto> GetCategoryHierarchy()
        {
            var allCategories = _categoryRepository.FindAll().ToList();
            var lookup = allCategories.ToLookup(c => c.ParentId);

            List<CategoryHierarchyDto> BuildTree(Guid? parentId)
            {
                return lookup[parentId]
                    .Select(c => new CategoryHierarchyDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        SubCategories = BuildTree(c.Id)
                    }).ToList();
            }

            return BuildTree(null); // Top-level categories (ParentId = null)

        }
        
        
    }
}
