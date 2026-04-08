using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Services;

public interface IProductCoreService
{
    // updated to support pagination, search, and filtering
    Task<PagedResult<Product>> GetAllProductsAsync(ProductQueryParameters query);
    
    Task<Product?> GetProductByIdAsync(string id);
    
    Task CreateProductAsync(Product product);
    
    Task UpdateProductAsync(string id, Product product);
    
    Task DeleteProductAsync(string id);
}