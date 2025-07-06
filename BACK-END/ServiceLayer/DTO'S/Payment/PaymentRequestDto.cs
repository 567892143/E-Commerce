
using ServiceLayer.Models;

namespace ServiceLayer.Payment.Dto;

public class PaymentRequestDto
{
    public PaymentMethod PaymentMethod { get; set; }  // Credit Card, PayPal, etc.
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
}
