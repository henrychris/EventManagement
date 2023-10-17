using ErrorOr;
using Shared.Responses;

namespace Shared.Extensions;

public static class ErrorOrExtensions
{
    public static ApiResponse<T> ToSuccessfulApiResponse<T>(this ErrorOr<T> errorOr)
    {
        return new ApiResponse<T>(data: errorOr.Value, message: "Success", status: true);
    }

    public static ApiResponse<T> ToSuccessfulApiResponse<T>(this ErrorOr<T> errorOr, string message)
    {
        return new ApiResponse<T>(data: errorOr.Value, message: message, status: true);
    }
}