using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Inventory.Dto;

namespace YourApp.Api.Controllers
{
    [ApiController]
    [CustomAuthorize("1")]
    [Route("api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET /api/inventory/{variantId}
        [HttpGet("{variantId}")]
        public IActionResult GetInventory(Guid variantId)
        {
            var inventory = _inventoryService.GetInventoryByVariantId(variantId);
            return inventory == null
                ? NotFound(new { message = "Inventory not found." })
                : Ok(inventory);
        }

        // POST /api/inventory
        [HttpPost]
        public IActionResult CreateInventory([FromBody] CreateInventoryDto dto)
        {
            _inventoryService.CreateInventory(dto);
            return Ok(new { message = "Inventory created successfully." });
        }

        // PUT /api/inventory/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateInventory(Guid id, [FromBody] UpdateInventoryDto dto)
        {
            var updated = _inventoryService.UpdateInventory(id, dto);
            return updated
                ? Ok(new { message = "Inventory updated successfully." })
                : NotFound(new { message = "Inventory not found." });
        }
    }
}
