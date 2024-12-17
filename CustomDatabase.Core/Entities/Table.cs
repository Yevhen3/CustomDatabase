using CustomDatabase.Core.Entities.Columns;

namespace CustomDatabase.Core.Entities;

public class Table(string name)
{
    private readonly List<Column> _columns = [];
    private readonly List<Row> _rows = [];
    
    public string Name => name;
    public IReadOnlyCollection<Column> Columns => _columns.AsReadOnly();
    public IReadOnlyCollection<Row> Rows => _rows.AsReadOnly();
    
    public void AddColumns(Column[] columns)
    {
        _columns.AddRange(columns);
    }
    
    public void AddColumn(Column column)
    {
        _columns.Add(column);
    }
    
    public void RemoveColumn(Column column)
    {
        _columns.Remove(column);
    }

    public void AddRow(Row row)
    {
        if (row.Values.Length != _columns.Count)
        {
            throw new ArgumentException("The number of values in the row must match the number of columns in the table.");
        }
        
        for (var i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].Type != row.Values[i].Type)
            {
                throw new ArgumentException("The type of the value must match the type of the column.");
            }
        }
        
        _rows.Add(row);
    }
}
