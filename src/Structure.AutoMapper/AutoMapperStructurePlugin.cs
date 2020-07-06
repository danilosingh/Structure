using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Structure.Application.Adapters;
using Structure.Cfg;
using System.Reflection;

namespace Structure.AutoMapper
{
    public class AutoMapperStructurePlugin : IStructurePlugin
    {
        private readonly Assembly[] assemblies;

        //TODO: refactor way to pass assemblies (use IOption)
        public AutoMapperStructurePlugin(Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        public void Configure(IStructureAppBuilder builder)
        {
            builder.Services.AddSingleton<IObjectAdapter, AutoMapperAdapter>();
            builder.Services.AddAutoMapper(assemblies);
        }
    }
}
