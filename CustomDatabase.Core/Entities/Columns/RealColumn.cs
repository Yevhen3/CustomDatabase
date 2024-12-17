

using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Columns;

public record RealColumn(string Name) : Column(Name, ColumnType.Real);