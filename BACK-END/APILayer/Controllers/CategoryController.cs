using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Product.Dto;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public ActionResult<List<ServiceLayer.Product.Dto.CategoryDto>> GetAll()
    {
        var categories = _categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDto> GetById(Guid id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpGet("hierarchy")]
    public ActionResult<List<CategoryHierarchyDto>> GetHierarchy()
    {
        var hierarchy = _categoryService.GetCategoryHierarchy();
        return Ok(hierarchy);
    }

    [HttpPost]
    [CustomAuthorize("1")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var categoryId = await _categoryService.CreateCategory(dto);
        return CreatedAtAction(nameof(GetById), new { id = categoryId }, null);
    }

    [HttpPut("{id}")]
    [CustomAuthorize("1")]
    public IActionResult Update(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var success = _categoryService.UpdateCategory(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [CustomAuthorize("1")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _categoryService.DeleteCategory(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
