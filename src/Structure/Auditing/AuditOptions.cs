using System.Collections.Generic;

namespace Structure.Auditing
{
    public class AuditOptions
    {
        public Dictionary<string, EntityAuditPropertyState> DefaultStates { get; }

        public AuditOptions()
        {
            DefaultStates = new Dictionary<string, EntityAuditPropertyState>();
        }
    }
}
