using System.Text.Json;
using CustomDatabase.Core.Entities.Serialization;
using CustomDatabase.Core.Extensions;

namespace CustomDatabase.Core.Entities;

public class Database(string name, string filePath)
{
    private readonly List<Table> _tables = [];

    public string Name => name;
    public string FilePath => filePath;
    public IReadOnlyCollection<Table> Tables => _tables.AsReadOnly();

    public void AddTables(Table[] tables)
    {
        _tables.AddRange(tables);
    }

    public void AddTable(Table table)
    {
        _tables.Add(table);
    }

    public void RemoveTable(Table table)
    {
        _tables.Remove(table);
    }

    public async Task ReadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var tablesJson = await File.ReadAllTextAsync(filePath, cancellationToken);
        var tables = JsonSerializer.Deserialize<List<SerializableTable>>(tablesJson);

        if (tables is null)
        {
            return;
        }

        foreach (var table in tables)
        {
            var newTable = table.ToTable();
            _tables.Add(newTable);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var serializedTables = _tables.Select(t => t.ToSerializableTable()).ToList();
        var tablesJson = JsonSerializer.Serialize(serializedTables);
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        await File.WriteAllTextAsync(filePath, tablesJson, cancellationToken);
    }
}