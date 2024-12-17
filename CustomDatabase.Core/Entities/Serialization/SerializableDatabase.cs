namespace CustomDatabase.Core.Entities.Serialization;

public class SerializableDatabase
{
    public string Name { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public List<SerializableTable> Tables { get; set; } = new List<SerializableTable>();
}