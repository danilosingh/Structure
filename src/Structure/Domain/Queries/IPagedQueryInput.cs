namespace Structure.Domain.Queries
{
    public interface IPagedQueryInput
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
