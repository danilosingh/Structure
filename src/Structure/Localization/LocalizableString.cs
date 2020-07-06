namespace Structure.Localization
{
    public class LocalizableString : ILocalizableString
    {
        public virtual string SourceName { get; private set; }
        public virtual string Name { get; private set; }

        public LocalizableString(string sourceName, string name)
        {
            SourceName = sourceName;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
