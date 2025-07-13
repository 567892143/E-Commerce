using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Product.Discount.Dto;
using ServiceLayer.Product.Dto;
using ServiceLayer.ProductDetail.Dto;
using ServiceLayer.UpsertProduct.Dto;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("by-category/{categoryId}")]
        public ActionResult<List<ProductDto>> GetProductsByCategory(Guid categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }

        [HttpGet("{id}/variants")]
        public ActionResult<List<VariantDto>> GetProductVariants(Guid id)
        {
            var variants = _productService.GetVariantsByProductId(id);
            return Ok(variants);
        }

        [HttpGet("{id}/images")]
        public ActionResult<List<ImageDto>> GetProductImages(Guid id)
        {
            var images = _productService.GetImagesByProductId(id);
            return Ok(images);
        }

        // Admin endpoints

        [HttpPost]
        [CustomAuthorize("1")]
        public ActionResult CreateProduct([FromBody] CreateProductDto productDto)
        {
            var productId = _productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, null);
        }

        [HttpPut("{id}")]
        [CustomAuthorize("1")]
        public IActionResult UpdateProduct(Guid id, [FromBody] UpdateProductDto updateDto)
        {
            var updated = _productService.UpdateProduct(id, updateDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [CustomAuthorize("1")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // GET /api/products/{id}/reviews
        [HttpGet("{productId}/reviews")]
        public ActionResult<List<ReviewDto>> GetProductReviews(Guid productId)
        {
            var reviews = _productService.GetReviewsByProductId(productId);
            return Ok(reviews);
        }

        // POST /api/products/{id}/reviews
        [HttpPost("{productId}/reviews")]
        public IActionResult AddReview(Guid productId, [FromBody] CreateReviewDto dto)
        {
            var userId = GetUserId();
            _productService.AddReview(userId, productId, dto);
            return Ok(new { message = "Review submitted successfully." });
        }

        // DELETE /api/reviews/{id}
        [HttpDelete("reviews/{reviewId}")]
        [CustomAuthorize("1")]
        public IActionResult DeleteReview(Guid reviewId)
        {
            var userId = GetUserId();
            var result = _productService.DeleteReview(userId, reviewId);
            return result ? NoContent() : Forbid();
        }

        // GET /api/discounts/validate?code=XXXX
        [HttpGet("discounts/validate")]
        [CustomAuthorize("1")]
        public IActionResult ValidateDiscount([FromQuery] string code)
        {
            var discount = _productService.ValidateDiscount(code);
            if (discount == null)
                return NotFound(new { message = "Invalid or expired discount code." });

            return Ok(discount);
        }

        // POST /api/discounts
        [HttpPost("discounts")]
        [CustomAuthorize("1")]
        public IActionResult CreateDiscount([FromBody] CreateDiscountDto dto)
        {
            _productService.CreateDiscount(dto);
            return Ok(new { message = "Discount created successfully." });
        }

        // POST /api/discounts/apply
        [HttpPost("discounts/apply")]
        public IActionResult ApplyDiscount([FromBody] ApplyDiscountDto dto)
        {
            var result = _productService.ApplyDiscount(dto);
            if (result == null)
                return BadRequest(new { message = "Discount not applicable." });

            return Ok(result);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("User ID not found"));
        }
    }
}
