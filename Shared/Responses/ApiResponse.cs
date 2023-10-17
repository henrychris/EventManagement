using System.Text.Json;

namespace Shared.Responses;

public class ApiResponse<T>
{
    public ApiResponse(T? data, string message, bool status)
    {
        Data = data;
        Message = message;
        Status = status;
    }

    public bool Status { get; set; }
    public string Message { get; set; }
    public string Note { get; set; } = "N/A";
    public T? Data { get; set; }

    public string ToJsonString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}