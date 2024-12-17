

using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Columns;

public record CharColumn(string Name) : Column(Name, ColumnType.Char);