using CustomDatabase.Core.Entities;
using CustomDatabase.Core.Entities.Columns;
using CustomDatabase.Core.Entities.Enums;
using CustomDatabase.Core.Entities.RowColumnValues;
using CustomDatabase.Core.Entities.Serialization;

namespace CustomDatabase.Core.Extensions;

public static class TableExtensions
{
    public static SerializableTable ToSerializableTable(this Table table)
    {
        var columns = table.Columns.Select(c => new SerializableColumn
        {
            Name = c.Name,
            Type = c.Type
        }).ToList();

        var serializableRows = table.Rows.Select(r =>
        {
            return new SerializableRow
            {
                Values = r.Values.Select(v =>
                {
                    return v.Type switch
                    {
                        ColumnType.Integer => new SeriazlizableRowColumnValue
                            { Type = ColumnType.Integer, Value = ((IntegerRowColumnValue)v).Value.ToString() },
                        ColumnType.String => new SeriazlizableRowColumnValue
                            { Type = ColumnType.String, Value = ((StringRowColumnValue)v).Value },
                        ColumnType.Char => new SeriazlizableRowColumnValue
                            { Type = ColumnType.Char, Value = ((CharRowColumnValue)v).Value.ToString() },
                        ColumnType.Real => new SeriazlizableRowColumnValue
                            { Type = ColumnType.Real, Value = ((RealRowColumnValue)v).Value.ToString() },
                        _ => throw new Exception("Unknown column type.")
                    };
                }).ToArray()
            };
        }).ToList();
            
        return new SerializableTable
        {
            Name = table.Name,
            Columns = columns,
            Rows = serializableRows
        };
    }
    
    public static Table ToTable(this SerializableTable serializableTable)
    {
        var columns = serializableTable.Columns.Select(c =>
        {
            return c.Type switch
            {
                ColumnType.Integer => new IntegerColumn(c.Name) as Column,
                ColumnType.String => new StringColumn(c.Name),
                ColumnType.Char => new CharColumn(c.Name),
                ColumnType.Real => new RealColumn(c.Name),
                _ => throw new Exception("Unknown column type.")
            };
        }).ToArray();
            
        var rows = serializableTable.Rows.Select(r =>
        {
            var rowColumnValues = r.Values.Select(v =>
            {
                return v.Type switch
                {
                    ColumnType.Integer => new IntegerRowColumnValue(int.Parse(v.Value!)) as RowColumnValue,
                    ColumnType.String => new StringRowColumnValue(v.Value!),
                    ColumnType.Char => new CharRowColumnValue(char.Parse(v.Value!)),
                    ColumnType.Real => new RealRowColumnValue(decimal.Parse(v.Value!)),
                    _ => throw new Exception("Unknown column type.")
                };
            }).ToArray();
                
            return new Row(rowColumnValues);
        }).ToArray();
            
        var newTable = new Table(serializableTable.Name);
        newTable.AddColumns(columns);

        foreach (var row in rows)
        {
            newTable.AddRow(row);
        }
        
        return newTable;
    }
}