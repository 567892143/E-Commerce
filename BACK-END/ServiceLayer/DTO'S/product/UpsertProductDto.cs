
namespace ServiceLayer.UpsertProduct.Dto;
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public List<CreateVariantDto> Variants { get; set; } = new();
    public List<CreateImageDto> Images { get; set; } = new();
}

    public class UpdateProductDto : CreateProductDto
    {
        public bool IsActive { get; set; }
    }
     public class CreateVariantDto
    {
        public string Sku { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class CreateImageDto
    {
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsPrimary { get; set; }
    }



