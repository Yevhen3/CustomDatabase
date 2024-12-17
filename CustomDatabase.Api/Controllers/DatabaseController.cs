using CustomDatabase.Api.Models;
using CustomDatabase.Core;
using CustomDatabase.Core.Entities;
using CustomDatabase.Core.Entities.Columns;
using CustomDatabase.Core.Entities.RowColumnValues;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/databases")]
public class DatabaseController : ControllerBase
{
    private static readonly DatabaseManager _databaseManager = new ();

    [HttpPost("create")]
    public IActionResult CreateDatabase([FromBody] CreateDatabaseRequest request)
    {
        var database = new Database(request.Name, request.FilePath);
        if (!_databaseManager.AddDatabase(database))
        {
            return BadRequest("Database with the same name already exists.");
        }

        _databaseManager.AddDatabase(database);

        return Ok(new { Message = "Database created successfully." });
    }

    [HttpPost("{databaseName}/table/add")]
    public IActionResult AddTable(string databaseName, [FromBody] AddTableRequest request)
    {
        var database = _databaseManager.GetDatabase(databaseName);
        if (database == null)
        {
            return BadRequest($"Database '{databaseName}' not found.");
        }

        var table = new Table(request.TableName);
        var columns = request.Columns.Select(c =>
        {
            return c.Type switch
            {
                "Integer" => new IntegerColumn(c.Name) as Column,
                "String" => new StringColumn(c.Name),
                "Char" => new CharColumn(c.Name),
                "Real" => new RealColumn(c.Name),
                _ => throw new ArgumentException("Invalid column type.")
            };
        }).ToArray();

        table.AddColumns(columns);
        database.AddTable(table);

        return Ok(new { Message = "Table added successfully." });
    }

    [HttpPost("{databaseName}/row/add")]
    public IActionResult AddRow(string databaseName, [FromBody] AddRowRequest request)
    {
        var database = _databaseManager.GetDatabase(databaseName);
        if (database == null)
        {
            return BadRequest($"Database '{databaseName}' not found.");
        }

        var table = database.Tables.FirstOrDefault(t => t.Name == request.TableName);
        if (table == null)
        {
            return BadRequest($"Table '{request.TableName}' not found in database '{databaseName}'.");
        }

        var rowValues = request.Values.Select(v =>
        {
            return v.Type switch
            {
                "Integer" => new IntegerRowColumnValue(int.Parse(v.Value)) as RowColumnValue,
                "String" => new StringRowColumnValue(v.Value),
                "Char" => new CharRowColumnValue(char.Parse(v.Value)),
                "Real" => new RealRowColumnValue(decimal.Parse(v.Value)),
                _ => throw new ArgumentException("Invalid value type.")
            };
        }).ToArray();

        var row = new Row(rowValues);
        table.AddRow(row);

        return Ok(new { Message = "Row added successfully." });
    }

    [HttpGet]
    public IActionResult GetDatabases()
    {
        if (_databaseManager.Databases.Count == 0)
        {
            return NotFound(new { Message = "No databases found." });
        }

        var databases = _databaseManager.Databases.Select(db => new
        {
            db.Name,
            db.FilePath,
            Tables = db.Tables.Select(t => new
            {
                t.Name,
                Columns = t.Columns.Select(c => new { c.Name, c.Type }),
                Rows = t.Rows.Select(r => r.Values.Select(v => v.Type + ": " + v.ToString()))
            })
        });

        return Ok(databases);
    }
    [HttpPost("load")]
    public IActionResult LoadDatabasesFromFile([FromQuery] string filePath = "databases.json")
    {
        _databaseManager.ReadFromFileAsync(CancellationToken.None); 
        return Ok(new { Message = "All databases readed successfully." });
    }



    [HttpPost("save")]
    public IActionResult SaveAllDatabases()
    {
        _databaseManager.SaveToFileAsync(CancellationToken.None); 
        return Ok(new { Message = "All databases saved successfully." });
    }
}