using CustomDatabase.Core.Entities.Enums;

namespace CustomDatabase.Core.Entities.Columns;

public abstract record Column(string Name, ColumnType Type);