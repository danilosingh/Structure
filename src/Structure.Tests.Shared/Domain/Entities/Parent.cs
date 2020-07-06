namespace Structure.Tests.Shared.Domain.Entities
{
    public abstract class Parent
    {
        public virtual  int Id { get; set; }
        public abstract ParentType Type { get; }
    }
}
