using Structure.Data.Filtering;

namespace Structure.Nhibernate.Filtering
{
    public static class DataFilterExtensions
    {
        public static NhDataFilter ToNhDataFilter(this IDataFilter dataFilter)
        {
            return NhDataFilterBuilder.Build(dataFilter);
        }
    }
}
