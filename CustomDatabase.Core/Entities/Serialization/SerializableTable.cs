namespace CustomDatabase.Core.Entities.Serialization;

public class SerializableTable
{
    public string Name { get; set; } = string.Empty;
    public List<SerializableColumn> Columns { get; set; } = [];
    public List<SerializableRow> Rows { get; set; } = [];
}