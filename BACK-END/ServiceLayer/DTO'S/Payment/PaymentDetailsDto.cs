
using ServiceLayer.Models;

namespace ServiceLayer.Payment.Dto;
public class PaymentDetailsDto
{
    public PaymentMethod PaymentMethod { get; set; } 
    public PaymentStatus PaymentStatus { get; set; } 
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
