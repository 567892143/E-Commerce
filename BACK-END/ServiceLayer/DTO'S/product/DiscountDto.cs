
using ServiceLayer.Models;

namespace ServiceLayer.Product.Discount.Dto;

public class DiscountDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DiscountType Type { get; set; }  // "Percentage" or "Fixed Amount"
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateDiscountDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DiscountType Type { get; set; } = DiscountType.Percentage; // or "Fixed Amount"
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ApplyDiscountDto
{
    public string Code { get; set; } = string.Empty;
    public decimal OrderTotal { get; set; }
}

public class DiscountResultDto
{
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; } 
    public decimal Value { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
