

using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Columns;

public record IntegerColumn(string Name) : Column(Name, ColumnType.Integer);