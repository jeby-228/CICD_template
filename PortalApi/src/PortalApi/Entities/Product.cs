using System;
using Volo.Abp.Domain.Entities;

namespace PortalApi.Entities;

/// <summary>
/// Sample Product entity to demonstrate ABP Framework with PostgreSQL
/// </summary>
public class Product : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    protected Product()
    {
        // Required by EF Core
    }
    
    public Product(Guid id, string name, decimal price, int stock)
        : base(id)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CreatedDate = DateTime.UtcNow;
    }
}
