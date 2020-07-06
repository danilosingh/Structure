namespace Structure.Localization
{
    public interface ILocalizableContext
    {
        ILocalizer Localizer { get; }
        ILocalizableString L(string name);
        ILocalizableString L(string sourceName, string name);
    }
}
