namespace Structure.Nhibernate
{
    public class NhibernateSchemaUpdateOptions
    {
        public bool DoUpdate { get; set; }
        public bool SaveToFile { get; set; }

        internal bool AllowsInvokeSchemaUpdate()
        {
            return DoUpdate || SaveToFile;
        }
    }
}