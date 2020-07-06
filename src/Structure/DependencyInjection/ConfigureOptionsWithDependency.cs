using System;

namespace Structure.AspNetCore.Cfg
{
    public class ConfigureOptionsWithDependency<TOptions, TDependency>
    {
        public Action<TOptions, TDependency> Action { get; set; }
    }
}
