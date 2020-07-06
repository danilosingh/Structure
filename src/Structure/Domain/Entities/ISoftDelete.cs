namespace Structure.Domain.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
