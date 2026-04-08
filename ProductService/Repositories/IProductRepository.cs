using ProductService.Models;
using ProductService.DTOs;

namespace ProductService.Repositories;

public interface IProductRepository
{
    // Update to accept query parameters and return tuple (Items, TotalCount)
    Task<(List<Product> Items, long TotalCount)> GetAllAsync(ProductQueryParameters query);
    
    Task<Product?> GetByIdAsync(string id);
    Task CreateAsync(Product product);
    Task UpdateAsync(string id, Product product);
    Task DeleteAsync(string id);
}