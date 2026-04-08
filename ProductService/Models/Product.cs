using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models;

public class Category
{
    [Required(ErrorMessage = "Category ID is required")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name is too long")]
    public string Name { get; set; } = string.Empty;
}

public class ProductVariant
{
    [Required(ErrorMessage = "Variant SKU is required")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "Color is required")]
    public string Color { get; set; } = string.Empty;

    [Required(ErrorMessage = "Size is required")]
    public string Size { get; set; } = string.Empty;
    
    [Range(0, 1000000000, ErrorMessage = "Price adjustment must be a positive number")]
    public decimal PriceAdjustment { get; set; } 
    
    [Range(0, 1000000, ErrorMessage = "Stock cannot be negative or exceed 1 million")]
    public int Stock { get; set; }
}

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    // Để string? để Swagger không bắt nhập ID khi POST
    public string? Id { get; set; }

    [Required(ErrorMessage = "Product name is mandatory")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product SKU is mandatory")]
    public string Sku { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1000, 1000000000, ErrorMessage = "Base price must be at least 1,000 VND")]
    public decimal BasePrice { get; set; }
    
    [Required(ErrorMessage = "Brand name is required")]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public Category Category { get; set; } = new Category();

    public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public List<string> Images { get; set; } = new List<string>();

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}