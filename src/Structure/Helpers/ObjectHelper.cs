using Structure.Extensions;
using System;


namespace Structure.Helpers
{
    public static class ObjectHelper
    {
        public static T GetInstance<T>(T obj)
        {
            if (obj == null)
            {
                obj = Activator.CreateInstance<T>();
            }

            return obj;
        }

        public static bool InstanceIs<T>(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType().InheritOrImplement(typeof(T), false);
        }

        public static T CreateInstance<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }

        public static T GetInstance<T>(ref T obj)
        {
            if (obj == null)
            {
                obj = Activator.CreateInstance<T>();
            }

            return obj;
        }

        public static T GetInstance<T>(ref T objeto, Func<T> constructor)
        {
            if (objeto == null)
            {
                objeto = constructor();
            }

            return objeto;
        }
    }
}
