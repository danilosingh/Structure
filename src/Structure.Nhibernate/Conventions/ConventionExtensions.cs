namespace Structure.Nhibernate.Conventions
{
    public static class ConventionExtensions
    {
        public static string ToConventionCase(this string name)
        {
            return FluentSessionMappingConfig.Instance.UseCamelCaseNames ? name.ToLower() : name;
        }
    }
}
