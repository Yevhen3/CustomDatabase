using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.RowColumnValues;

public record StringRowColumnValue(string Value) : RowColumnValue<string>(Value, ColumnType.String);