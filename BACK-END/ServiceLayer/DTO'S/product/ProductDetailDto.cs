using ServiceLayer.Product.Dto;

namespace ServiceLayer.ProductDetail.Dto;

public class ProductDetailDto : ProductDto
{
    public List<VariantDto> Variants { get; set; } = new();
    public List<ImageDto> Images { get; set; } = new();
}
    public class VariantDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsPrimary { get; set; }
    }