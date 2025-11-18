using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(AbpDddDomainModule)
)]
public class PortalApiDomainModule : AbpModule
{
}
