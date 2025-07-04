using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Product.Dto;

namespace YourApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("categories")]
        public ActionResult<List<CategoryDto>> GetAllCategories()
        {
            var categories = _productService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("all")]
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
