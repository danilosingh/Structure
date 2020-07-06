using Microsoft.Extensions.DependencyInjection;

namespace Structure.Cfg
{
    public interface IStructureModule
    {
        void ConfigureServices(IServiceCollection services);
        void Configure();
    }
}