using Microsoft.Extensions.Logging;
using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Order.Dto;
using ServiceLayer.Payment.Dto;
using ServiceLayer.Shipping.Dto;
using Shared.Services.Exceptions;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IShippingRepository _shippingRepo;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepo,
            IPaymentRepository paymentRepo,
            IShippingRepository shippingRepo,
            ILogger<OrderService> logger)
        {
            _orderRepo = orderRepo;
            _paymentRepo = paymentRepo;
            _shippingRepo = shippingRepo;
            _logger = logger;
        }

        // ------------------- ORDERS -------------------

        public Guid PlaceOrder(Guid userId, CreateOrderDto dto)
        {
            var orderId = Guid.NewGuid();

            // Order + OrderItems creation
            var order = new Models.Order
            {
                Id = orderId,
                UserId = userId,
                Subtotal = 0, // will calculate
                Tax = 0,
                Discount = 0,
                ShippingCost = 50, // static or from config
                TotalAmount = 0,
                Status = OrderStatus.Pending,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            order.Items = dto.Items.Select(i =>
            {
                var price = _orderRepo.GetVariantPrice(i.VariantId);
                return new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    VariantId = i.VariantId,
                    Quantity = i.Quantity,
                    UnitPrice = price,
                    TotalPrice = price * i.Quantity,
                    CreatedAt = DateTime.UtcNow
                };
            }).ToList();

            order.Subtotal = order.Items.Sum(i => i.TotalPrice);
            order.TotalAmount = order.Subtotal + order.ShippingCost - order.Discount + order.Tax;

            _orderRepo.Create(order);
            _orderRepo.SaveAsync();

            return orderId;
        }

        public List<OrderSummaryDto> GetOrdersByUserId(Guid userId)
        {
            return _orderRepo.GetOrdersByUserId(userId)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                }).ToList();
        }

        public OrderDetailsDto? GetOrderById(Guid orderId, Guid userId)
        {
            var order = _orderRepo.GetOrderWithItems(orderId, userId);
            if (order == null) return null;

            return new OrderDetailsDto
            {
                Id = order.Id,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                ShippingCost = order.ShippingCost,
                Discount = order.Discount,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDetailsDto
                {
                    VariantId = i.VariantId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    ProductName = i.Variant.Product.Name,
                    Size = i.Variant.Size,
                    Color = i.Variant.Color
                }).ToList()
            };
        }

        public bool CancelOrder(Guid orderId, Guid userId)
        {
            var order = _orderRepo.FindByCondition(o => o.Id == orderId).FirstOrDefault();
            if (order == null || order.UserId != userId || order.Status != OrderStatus.Pending) return false;

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepo.Update(order);
            _orderRepo.SaveAsync();
            return true;
        }

        public bool UpdateOrderStatus(Guid orderId, UpdateOrderStatusDto dto)
        {
            var order = _orderRepo.FindByCondition(o => o.Id == orderId).FirstOrDefault();
            if (order == null) return false;

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepo.Update(order);
            _orderRepo.SaveAsync();
            return true;
        }

        // ------------------- PAYMENT -------------------

        public bool ProcessPayment(Guid orderId, PaymentRequestDto dto)
        {
            var payment = new Models.Payment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethod = dto.PaymentMethod,
                Amount = dto.Amount,
                PaymentStatus = PaymentStatus.Completed,
                TransactionId = dto.TransactionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _paymentRepo.Create(payment);
            return true;
        }

        public PaymentDetailsDto? GetPaymentDetails(Guid orderId)
        {
            var payment = _paymentRepo.GetByOrderId(orderId);
            if (payment == null) return null;

            return new PaymentDetailsDto
            {
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                CreatedAt = payment.CreatedAt
            };
        }

        // ------------------- SHIPPING -------------------

        public ShippingDetailsDto? GetShippingDetails(Guid orderId)
        {
            var shipping = _shippingRepo.GetByOrderId(orderId);
            if (shipping == null) return null;

            return new ShippingDetailsDto
            {
                Carrier = shipping.Carrier,
                TrackingNumber = shipping.TrackingNumber,
                Status = shipping.Status,
                EstimatedDelivery = shipping.EstimatedDelivery,
                ActualDelivery = shipping.ActualDelivery
            };
        }

        public bool UpdateShippingDetails(Guid orderId, UpdateShippingDto dto)
        {
            var shipping = _shippingRepo.GetByOrderId(orderId);
            if (shipping == null) return false;

            shipping.Carrier = dto.Carrier;
            shipping.TrackingNumber = dto.TrackingNumber;
            shipping.Status = dto.Status;
            shipping.EstimatedDelivery = dto.EstimatedDelivery;
            shipping.UpdatedAt = DateTime.UtcNow;

            _shippingRepo.Update(shipping);
            _shippingRepo.SaveAsync();

            return true;

        }
    }
}
