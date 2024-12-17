using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.RowColumnValues;

public record RealRowColumnValue(decimal Value) : RowColumnValue<decimal>(Value, ColumnType.Real);