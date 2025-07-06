using ServiceLayer.Order.Dto;
using ServiceLayer.Payment.Dto;
using ServiceLayer.Shipping.Dto;

namespace ServiceLayer.Interface
{
    public interface IOrderService
    {
        // Orders
        Guid PlaceOrder(Guid userId, CreateOrderDto dto);
        List<OrderSummaryDto> GetOrdersByUserId(Guid userId);
        OrderDetailsDto? GetOrderById(Guid orderId, Guid userId);
        bool CancelOrder(Guid orderId, Guid userId);
        bool UpdateOrderStatus(Guid orderId, UpdateOrderStatusDto dto);

        // Payment
        bool ProcessPayment(Guid orderId, PaymentRequestDto dto);
        PaymentDetailsDto? GetPaymentDetails(Guid orderId);

        // Shipping
        ShippingDetailsDto? GetShippingDetails(Guid orderId);
        bool UpdateShippingDetails(Guid orderId, UpdateShippingDto dto);
    }
}
