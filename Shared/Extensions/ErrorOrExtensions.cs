using ErrorOr;
using Shared.Responses;

namespace Shared.Extensions;

public static class ErrorOrExtensions
{
    /// <summary>
    /// Create a successful ApiResponse object using the value from the ErrorOr object.
    /// </summary>
    /// <param name="errorOr">The ErrorOr result to convert.</param>
    /// <typeparam name="T">The type of data in the ApiResponse.</typeparam>
    /// <returns>A successful ApiResponse object with the provided data.</returns>
    /// <remarks>Only use this for successful scenarios, else, the data property would be null.</remarks>
    public static ApiResponse<T> ToSuccessfulApiResponse<T>(this ErrorOr<T> errorOr)
    {
        return new ApiResponse<T>(data: errorOr.Value, message: "Success", success: true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorOr">The ErrorOr result to convert.</param>
    /// <param name="message">A custom success message.</param>
    /// <typeparam name="T">The type of data in the ApiResponse.</typeparam>
    /// <returns>A successful ApiResponse object with the provided data.</returns>
    /// <remarks>Only use this for successful scenarios, else, the data property would be null.</remarks>
    public static ApiResponse<T> ToSuccessfulApiResponse<T>(this ErrorOr<T> errorOr, string message)
    {
        return new ApiResponse<T>(data: errorOr.Value, message: message, success: true);
    }

    /// <summary>
    /// Determines whether the specified ErrorOr instance has any errors.
    /// </summary>
    /// <typeparam name="T">The type of the value in the ErrorOr instance.</typeparam>
    /// <param name="errorOr">The ErrorOr instance to check for errors.</param>
    /// <returns>true if the ErrorOr instance has errors; otherwise, false.</returns>
    public static bool HasErrors<T>(this ErrorOr<T> errorOr)
    {
        return errorOr.Errors.Count > 0;
    }
}