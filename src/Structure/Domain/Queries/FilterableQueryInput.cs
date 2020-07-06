namespace Structure.Domain.Queries
{
    public class FilterableQueryInput : PagedAndSortedQueryInput, IFilteredQueryInput
    {
        public string[] Fields { get; set; }
        public string FilterText { get; set; }

        public virtual bool HasFields()
        {
            return Fields != null && Fields.Length > 0;
        }
    }
}
