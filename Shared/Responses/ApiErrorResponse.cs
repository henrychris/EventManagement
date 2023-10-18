using System.Text.Json;

namespace Shared.Responses;

public class ApiErrorResponse<T>
{
    public ApiErrorResponse(T errors, string message)
    {
        Errors = errors;
        Message = message;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public T Errors { get; set; }

    public string ToJsonString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}