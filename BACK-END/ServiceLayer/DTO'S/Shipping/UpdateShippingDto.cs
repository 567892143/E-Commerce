using ServiceLayer.Models;

namespace ServiceLayer.Shipping.Dto;
public class UpdateShippingDto
{
    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public ShippingStatus Status {get; set; } 
    public DateTime EstimatedDelivery { get; set; }
}
