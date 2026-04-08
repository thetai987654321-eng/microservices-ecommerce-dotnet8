namespace ProductService.DTOs;

public class ProductQueryParameters
{
    // Search & Filter
    public string? SearchTerm { get; set; }
    public string? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Sorting (e.g., price_asc, price_desc, name_asc)
    public string? SortBy { get; set; } 

    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}