using Microsoft.Extensions.Options;

namespace Structure.AspNetCore.Cfg
{
    public class ConfigureOptionsWithDependencyWrapper<TOptions, TDependency> : IConfigureOptions<TOptions>
        where TOptions : class, new()
    {
        public ConfigureOptionsWithDependencyWrapper(TDependency dependency, IOptions<ConfigureOptionsWithDependency<TOptions, TDependency>> configureOptionsWithDependency)
        {
            ConfigureOptionsWithDependency = configureOptionsWithDependency.Value;
            Dependency = dependency;
        }

        public ConfigureOptionsWithDependency<TOptions, TDependency> ConfigureOptionsWithDependency { get; set; }

        public TDependency Dependency { get; set; }

        public void Configure(TOptions options)
        {
            ConfigureOptionsWithDependency.Action(options, Dependency);
        }
    }
}
