namespace Structure.Auditing
{
    public class EntityAuditPropertyState
    {
        public bool IsEnabled { get; set; }

        public EntityAuditPropertyState(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        public EntityAuditPropertyState Clone()
        {
            return new EntityAuditPropertyState(IsEnabled);
        }
    }
}
