namespace CustomDatabase.Api.Models;

public class AddTableRequest
{
    public string TableName { get; set; }
    public List<ColumnRequest> Columns { get; set; }
}