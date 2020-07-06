using System.Collections.ObjectModel;

namespace Structure.Validation
{
    public class ValidationErrorCollection : Collection<ValidationError>
    {
        public void Add(string message, string member)
        {
            Items.Add(new ValidationError(message, member));
        }

        public void Add(string message)
        {
            Items.Add(new ValidationError(message));
        }
    }
}
