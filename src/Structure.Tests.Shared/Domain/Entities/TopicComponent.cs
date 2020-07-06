namespace Structure.Tests.Shared.Entities
{
    public class TopicComponent
    {
        public virtual string Value { get; set; }
        public TopicChildComponent Child { get; set; }
    }
}