namespace EntityBuilder
{
    public sealed class TableDefinition
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public ColumnDefinition[] ColumnDefinitions { get; set; }
    }
}