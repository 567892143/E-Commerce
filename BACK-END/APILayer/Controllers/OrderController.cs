using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Order.Dto;
using ServiceLayer.Payment.Dto;
using ServiceLayer.Shipping.Dto;

namespace YourApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ------------- Orders -------------

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] CreateOrderDto dto)
        {
            var userId = GetUserId();
            var orderId = _orderService.PlaceOrder(userId, dto);
            return Ok(new { OrderId = orderId });
        }

        [HttpGet]
        public ActionResult<List<OrderSummaryDto>> GetUserOrders()
        {
            var userId = GetUserId();
            var orders = _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDetailsDto> GetOrderById(Guid id)
        {
            var userId = GetUserId();
            var order = _orderService.GetOrderById(id, userId);
            return order != null ? Ok(order) : NotFound();
        }

        [HttpPut("{id}/cancel")]
        public IActionResult CancelOrder(Guid id)
        {
            var userId = GetUserId();
            var result = _orderService.CancelOrder(id, userId);
            return result ? NoContent() : BadRequest("Cannot cancel order");
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            // Admin-only (auth logic not shown here)
            var result = _orderService.UpdateOrderStatus(id, dto);
            return result ? NoContent() : BadRequest("Failed to update order status");
        }

        // ------------- Payments -------------

        [HttpPost("{id}/pay")]
        public IActionResult MakePayment(Guid id, [FromBody] PaymentRequestDto dto)
        {
            var result = _orderService.ProcessPayment(id, dto);
            return result ? Ok() : BadRequest("Payment failed");
        }

        [HttpGet("{id}/payment")]
        public ActionResult<PaymentDetailsDto> GetPayment(Guid id)
        {
            var payment = _orderService.GetPaymentDetails(id);
            return payment != null ? Ok(payment) : NotFound();
        }

        // ------------- Shipping -------------

        [HttpGet("{id}/shipping")]
        public ActionResult<ShippingDetailsDto> GetShipping(Guid id)
        {
            var shipping = _orderService.GetShippingDetails(id);
            return shipping != null ? Ok(shipping) : NotFound();
        }

        [HttpPut("{id}/shipping")]
        public IActionResult UpdateShipping(Guid id, [FromBody] UpdateShippingDto dto)
        {
            // Admin-only
            var result = _orderService.UpdateShippingDetails(id, dto);
            return result ? NoContent() : BadRequest("Failed to update shipping");
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("User ID not found"));
        }
    }
}
