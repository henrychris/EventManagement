using System.Text.Json;

namespace Shared;

public sealed class InternalErrorResponse
{
    /// <summary>
    /// error status
    /// </summary>
    public string Status { get; set; } = "Internal Server Error!";

    /// <summary>
    /// http status code of the error response
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// the reason for the error
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// additional information or instructions
    /// </summary>
    public string Note { get; set; } = string.Empty;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}