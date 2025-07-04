using System;

namespace ServiceLayer.Models
{
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        COD,
        BankTransfer
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string TransactionId { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Order? Order { get; set; }
    }
}
