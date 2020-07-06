namespace Structure.Localization
{
    public class LocalizableContext : ILocalizableContext
    {
        public ILocalizer Localizer { get; }

        public LocalizableContext(ILocalizer localizer)
        {
            Localizer = localizer;
        }

        public ILocalizableString L(string name)
        {
            return L(Localizer.Options.Value.DefaultSourceName, name);
        }

        public ILocalizableString L(string sourceName, string name)
        {
            return new LocalizableString(sourceName, name);
        }
    }
}
