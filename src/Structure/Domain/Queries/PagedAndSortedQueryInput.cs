namespace Structure.Domain.Queries
{
    public class PagedAndSortedQueryInput : IPagedAndSortedQueryInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public int Skip
        {
            get { return Page <= 0 ? 0 : (Page - 1) * PageSize; }
        }

        public PagedAndSortedQueryInput()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
