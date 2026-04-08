using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using ProductService.Configurations;
using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _productsCollection = database.GetCollection<Product>(mongoDbSettings.Value.ProductsCollectionName);
    }

    public async Task<(List<Product> Items, long TotalCount)> GetAllAsync(ProductQueryParameters query)
    {
        var filterBuilder = Builders<Product>.Filter;
        
        // default filter: active and not deleted
        var filter = filterBuilder.Eq(p => p.IsDeleted, false) 
                   & filterBuilder.Eq(p => p.IsActive, true);

        // 1. search by name
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            filter &= filterBuilder.Regex(p => p.Name, new BsonRegularExpression(query.SearchTerm, "i"));
        }

        // 2. filter by category id
        if (!string.IsNullOrWhiteSpace(query.CategoryId))
        {
            // fix: map to Category.Id based on your current JSON model
            filter &= filterBuilder.Eq(p => p.Category.Id, query.CategoryId);
        }

        // 3. filter by price range
        if (query.MinPrice.HasValue)
        {
            // fix: map to BasePrice based on your current JSON model
            filter &= filterBuilder.Gte(p => p.BasePrice, query.MinPrice.Value);
        }
        if (query.MaxPrice.HasValue)
        {
            // fix: map to BasePrice
            filter &= filterBuilder.Lte(p => p.BasePrice, query.MaxPrice.Value);
        }

        var findFluent = _productsCollection.Find(filter);

        // 4. sorting
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            findFluent = query.SortBy.ToLower() switch
            {
                // fix: map to BasePrice
                "price_asc" => findFluent.SortBy(p => p.BasePrice),
                "price_desc" => findFluent.SortByDescending(p => p.BasePrice),
                "name_asc" => findFluent.SortBy(p => p.Name),
                "name_desc" => findFluent.SortByDescending(p => p.Name),
                _ => findFluent // default
            };
        }

        // 5. get total count for pagination
        var totalCount = await findFluent.CountDocumentsAsync();

        // 6. pagination (skip & limit)
        var items = await findFluent
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Limit(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _productsCollection
            .Find(p => p.Id == id && p.IsDeleted == false)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        await _productsCollection.InsertOneAsync(product);
    }

    public async Task UpdateAsync(string id, Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        await _productsCollection.ReplaceOneAsync(p => p.Id == id, product);
    }

    public async Task DeleteAsync(string id)
    {
        // soft delete implementation
        var update = Builders<Product>.Update
            .Set(p => p.IsDeleted, true)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        await _productsCollection.UpdateOneAsync(p => p.Id == id, update);
    }
}