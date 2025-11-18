using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalApi.Entities;
using PortalApi.EntityFrameworkCore;
using Volo.Abp.AspNetCore.Mvc;

namespace PortalApi.Controllers;

/// <summary>
/// Sample Products API controller demonstrating ABP Framework with PostgreSQL
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductsController : AbpController
{
    private readonly PortalApiDbContext _dbContext;

    public ProductsController(PortalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _dbContext.Products.ToListAsync();
        return Ok(products);
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto dto)
    {
        var product = new Product(
            Guid.NewGuid(),
            dto.Name,
            dto.Price,
            dto.Stock
        )
        {
            Description = dto.Description
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] CreateProductDto dto)
    {
        var product = await _dbContext.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}

/// <summary>
/// DTO for creating/updating products
/// </summary>
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
