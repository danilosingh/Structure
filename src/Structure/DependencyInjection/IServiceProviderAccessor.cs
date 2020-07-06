using System;

namespace Structure.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}
