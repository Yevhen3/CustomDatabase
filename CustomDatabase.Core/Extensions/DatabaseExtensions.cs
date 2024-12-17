using CustomDatabase.Core.Entities;
using CustomDatabase.Core.Entities.Serialization;

namespace CustomDatabase.Core.Extensions;

public static class DatabaseExtensions
{
    public static SerializableDatabase ToSerializableDatabase(this Database database)
    {
        var serializableTables = database.Tables.Select(t => t.ToSerializableTable()).ToList();

        return new SerializableDatabase
        {
            Name = database.Name,
            FilePath = database.FilePath,
            Tables = serializableTables
        };
    }

    public static Database ToDatabase(this SerializableDatabase serializableDatabase)
    {
        var database = new Database(serializableDatabase.Name, serializableDatabase.FilePath);
        foreach (var serializableTable in serializableDatabase.Tables)
        {
            var table = serializableTable.ToTable();
            database.AddTable(table);
        }
        return database;
    }
}
