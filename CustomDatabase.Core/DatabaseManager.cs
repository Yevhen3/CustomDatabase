using System.Text.Json;
using CustomDatabase.Core.Entities;
using CustomDatabase.Core.Entities.Serialization;
using CustomDatabase.Core.Extensions;

namespace CustomDatabase.Core;

public class DatabaseManager
{
    private readonly Dictionary<string, Database> _databases = new();
    private const string StorageFilePath = "MyDatabase.json"; 


    public IReadOnlyCollection<Database> Databases => _databases.Values;

    public bool AddDatabase(Database database)
    {
        if (_databases.ContainsKey(database.Name))
        {
            return false; // База даних з таким ім'ям вже існує
        }

        _databases[database.Name] = database;
        return true;
    }

    public bool RemoveDatabase(string name)
    {
        return _databases.Remove(name);
    }
    public List<SerializableDatabase> ToSerializable()
    {
        return Databases.Select(db => db.ToSerializableDatabase()).ToList();
    }
    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return; // Якщо файл не існує, нічого не завантажуємо
        }

        var fileContent = File.ReadAllText(filePath);
        var loadedDatabases = JsonSerializer.Deserialize<List<SerializableDatabase>>(fileContent);

        if (loadedDatabases != null)
        {
            foreach (var serializableDatabase in loadedDatabases)
            {
                var database = serializableDatabase.ToDatabase();
                AddDatabase(database);
            }
        }
    }



    public Database? GetDatabase(string name)
    {
        _databases.TryGetValue(name, out var database);
        return database;
    }

    public async Task SaveToFileAsync(CancellationToken cancellationToken)
    {
        var serializedDatabases = Databases.Select(db => db.ToSerializableDatabase()).ToList();
        var databasesJson = JsonSerializer.Serialize(serializedDatabases);

        await File.WriteAllTextAsync(StorageFilePath, databasesJson, cancellationToken);
    }
    
    public async Task ReadFromFileAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(StorageFilePath))
        {
            throw new FileNotFoundException("Storage file not found.", StorageFilePath);
        }

        var databasesJson = await File.ReadAllTextAsync(StorageFilePath, cancellationToken);
        var serializedDatabases = JsonSerializer.Deserialize<List<SerializableDatabase>>(databasesJson);

        if (serializedDatabases == null)
        {
            throw new InvalidOperationException("Failed to deserialize databases from file.");
        }

        _databases.Clear(); // Очищаємо словник перед завантаженням нових даних

        foreach (var serializedDatabase in serializedDatabases)
        {
            var database = serializedDatabase.ToDatabase();
            _databases[database.Name] = database; // Додаємо базу даних до словника
        }
    }


}
