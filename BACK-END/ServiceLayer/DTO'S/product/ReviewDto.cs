
namespace ServiceLayer.Product.Dto;
public class CreateReviewDto
{
    public int Rating { get; set; } // 1 to 5
    public string Comment { get; set; } = string.Empty;
}
public class ReviewDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsVerifiedPurchase { get; set; }
}
