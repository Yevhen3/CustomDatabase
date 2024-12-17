using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.RowColumnValues;

public record IntegerRowColumnValue(int Value) : RowColumnValue<int>(Value, ColumnType.Integer);