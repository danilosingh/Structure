namespace Structure.Domain.Entities.Auditing
{
    public interface IAudited : ICreationAudited, IModificationAudited
    { }
}