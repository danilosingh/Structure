using System;

namespace Structure.Helpers
{
    public static class GuidHelper
    {
        public static bool IsNullOrEmpty(Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        public static bool IsEmpty(Guid guid)
        {
            return guid == Guid.Empty;
        }
    }
}
