using Structure.Tests.Shared.Entities;

namespace Structure.Tests.Shared.Domain.Entities
{
    public class Child : Parent
    {
        public virtual  string ChildValue { get; set; }
        public virtual Role Role { get; set; }
        public override ParentType Type
        {
            get
            {
                return ParentType.Type1;
            }
        }
    }
}
