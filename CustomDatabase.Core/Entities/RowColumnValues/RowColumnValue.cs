using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.RowColumnValues;

public record RowColumnValue(ColumnType Type);

public record RowColumnValue<T>(T Value, ColumnType Type) : RowColumnValue(Type);