namespace Structure.Application.Adapters
{
    public interface IObjectAdapter
    {
        TDestination To<TDestination>(object source);
        TDestination To<TSource, TDestination>(TSource source, TDestination destination);
    }
}
