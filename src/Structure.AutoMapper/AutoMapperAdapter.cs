using AutoMapper;
using Structure.Application.Adapters;

namespace Structure.AutoMapper
{
    public class AutoMapperAdapter : IObjectAdapter
    {
        private readonly IMapper mapper;

        public AutoMapperAdapter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination To<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }

        public TDestination To<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map(source, destination);
        }
    }
}