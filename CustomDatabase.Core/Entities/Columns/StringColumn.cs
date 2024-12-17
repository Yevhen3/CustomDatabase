

using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Columns;

public record StringColumn(string Name) : Column(Name, ColumnType.String);