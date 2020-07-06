using Structure.Cfg;

namespace Structure.AspNetCore
{
    public interface IStructureAspNetAddOn
    {
        void Configure(IStructureAppBuilder builder);
    }
}
