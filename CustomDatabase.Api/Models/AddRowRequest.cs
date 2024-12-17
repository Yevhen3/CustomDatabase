namespace CustomDatabase.Api.Models;

public class AddRowRequest
{
    public string TableName { get; set; }
    public List<RowValueRequest> Values { get; set; }
}