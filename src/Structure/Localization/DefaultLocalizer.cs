using Microsoft.Extensions.Options;

namespace Structure.Localization
{
    public class DefaultLocalizer : ILocalizer
    {
        public IOptions<LocalizationOptions> Options { get; }

        public DefaultLocalizer(IOptions<LocalizationOptions> options)
        {
            Options = options;
        }
    }
}
