using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.RowColumnValues;

public record CharRowColumnValue(char Value) : RowColumnValue<char>(Value, ColumnType.Char);