using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PortalApi.EntityFrameworkCore;

public class PortalApiDbContext : AbpDbContext<PortalApiDbContext>
{
    public PortalApiDbContext(DbContextOptions<PortalApiDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure your entity mappings here
    }
}
