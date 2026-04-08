using ProductService.DTOs;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services;

public class ProductCoreService : IProductCoreService
{
    private readonly IProductRepository _productRepository;

    public ProductCoreService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> GetAllProductsAsync(ProductQueryParameters query)
    {
        // retrieve items and total count from repository
        var (items, totalCount) = await _productRepository.GetAllAsync(query);

        // map results to paged result DTO
        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task CreateProductAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        await _productRepository.CreateAsync(product);
    }

    public async Task UpdateProductAsync(string id, Product product)
    {
        await _productRepository.UpdateAsync(id, product);
    }

    public async Task DeleteProductAsync(string id)
    {
        await _productRepository.DeleteAsync(id);
    }
}
