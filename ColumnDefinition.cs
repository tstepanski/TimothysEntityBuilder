using System;

namespace EntityBuilder
{
    public sealed class ColumnDefinition
    {
        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool? IsIdentity { get; set; }
        public int Position { get; set; }
        public bool IsNullAllowed { get; set; }
        public Type Type { get; set; }
        public int? MaximumLength { get; set; }
        public byte? Precision { get; set; }
        public int? Scale { get; set; }
        public short? DateTimePrecision { get; set; }
    }
}