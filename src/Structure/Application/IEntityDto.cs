namespace Structure.Application
{
    public interface IEntityDto<TId>
    {
        TId Id { get; set; }
    }
}
