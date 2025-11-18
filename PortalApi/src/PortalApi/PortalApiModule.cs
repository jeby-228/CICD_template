using Microsoft.EntityFrameworkCore;
using PortalApi.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class PortalApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        // Configure Controllers
        context.Services.AddControllers();
        
        // Configure EF Core with PostgreSQL
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
        
        // Register DbContext
        context.Services.AddAbpDbContext<PortalApiDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });
        
        // Configure Database Connection String
        context.Services.AddDbContext<PortalApiDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseConfiguredEndpoints();
    }
}
