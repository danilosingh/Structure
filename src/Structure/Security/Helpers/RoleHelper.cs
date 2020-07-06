using System.Linq;

namespace Structure.Security.Helpers
{
    public class RoleHelper
    {
        public static string[] GetRoles<T>(params string[] actions)
        {
            return actions.Select(c => GetRole<T>(c)).ToArray();
        }

        public static string GetRole<T>(string action)
        {
            return GetRole(typeof(T).Name, action);
        }

        public static string[] GetRoles(string artefato, params string[] actions)
        {
            return actions.Select(c => GetRole(artefato, c)).ToArray();
        }

        public static string GetRole(string artefato, string action)
        {
            return artefato + action;
        }
    }
}
