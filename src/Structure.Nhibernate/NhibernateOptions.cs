namespace Structure.Nhibernate
{
    public class NhibernateOptions 
    {
        public NhibernateSchemaUpdateOptions SchemaUpdate { get;set; }
        public string[] MappingAssemblies { get; set; }
        public NhibernateDialect Dialect { get; set; }
        public string DefaultSchema { get; set; }
        public string ConnectionStringName { get; set; }

        public NhibernateOptions()
        {
            SchemaUpdate = new NhibernateSchemaUpdateOptions();
        }
    }
}
