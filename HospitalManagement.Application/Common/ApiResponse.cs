namespace HospitalManagement.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<object>? Errors { get; set; }
    public int StatusCode { get; set; }
    public string? TraceId { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "Success",
        int statusCode = 200,
        string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = statusCode,
            TraceId = traceId
        };
    }

    public static ApiResponse<T> Fail(
        string message,
        int statusCode = 400,
        IEnumerable<object>? errors = null,
        string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors,
            TraceId = traceId
        };
    }
}
