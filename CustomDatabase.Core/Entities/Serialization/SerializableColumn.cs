using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Serialization;

public class SerializableColumn
{
    public ColumnType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}