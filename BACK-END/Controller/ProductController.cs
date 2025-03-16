using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
      

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<product>>> GetProducts()
        {
            var products = new List<product>{
                new product{Id = 1, Name = "Product 1"},
                new product{Id = 2, Name = "Product 2"},
            };
            return Ok(products);
        }
    }

    public class product{
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
