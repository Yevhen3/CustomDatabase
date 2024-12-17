using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Serialization;

public class SeriazlizableRowColumnValue
{
    public ColumnType Type { get; set; }
    public string? Value { get; set; }
}