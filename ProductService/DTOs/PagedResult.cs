namespace ProductService.DTOs;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public long TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    
    // Auto-calculate total pages
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}