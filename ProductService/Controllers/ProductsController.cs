using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Services;
using ProductService.DTOs;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductCoreService _productService;

    public ProductsController(IProductCoreService productService)
    {
        _productService = productService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> GetAll([FromQuery] ProductQueryParameters query)
    {
        // No try-catch needed; Middleware handles unexpected crashes
        var result = await _productService.GetAllProductsAsync(query);
        return Ok(result);
    }

    // GET: api/Products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(string id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null) 
            return NotFound(new { message = $"Product with ID {id} not found" });

        return Ok(product);
    }

    // POST: api/Products
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        // Data Annotations in Product.cs handle validation automatically
        await _productService.CreateProductAsync(product);
        
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: api/Products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Product product)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);
        
        if (existingProduct == null) 
            return NotFound(new { message = "Cannot update: Product not found" });

        product.Id = id; // Ensure ID consistency
        await _productService.UpdateProductAsync(id, product);
        
        return NoContent(); 
    }

    // DELETE: api/Products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);
        
        if (existingProduct == null) 
            return NotFound(new { message = "Cannot delete: Product not found" });

        await _productService.DeleteProductAsync(id);
        
        return NoContent();
    }
}