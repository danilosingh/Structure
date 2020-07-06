using System;

namespace Structure.Auditing
{
    public interface IEntityPropertySetter
    {
        IDisposable Enable(string propertyName);
        IDisposable Disable(string propertyName);

        void SetCreationProperties(object targetObject);
        void SetModificationProperties(object targetObject);
        void SetDeletionProperties(object targetObject);
    }
}
