using Microsoft.Extensions.Options;

namespace Structure.Localization
{
    public interface ILocalizer
    {
        IOptions<LocalizationOptions> Options { get; }
    }
}
