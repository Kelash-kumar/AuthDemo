namespace AuthDemo.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }


        public static ApiResponse<T> Ok(T data, string message = "Request completed successfully.")
            => new() { Success = true, Message = message, Data = data, StatusCode = 200 };

        public static ApiResponse<T> Created(T data, string message = "Resource created successfully.")
            => new() { Success = true, Message = message, Data = data, StatusCode = 201 };

        public static ApiResponse<object> Fail(string message, int statusCode = 400, List<string>? errors = null)
            => new() { Success = false, Message = message, StatusCode = statusCode, Data = errors ?? new List<string>() };

        public static ApiResponse<object> NotFound(string message = "Resource not found.")
            => Fail(message, 404);

        public static ApiResponse<object> Unauthorized(string message = "Unauthorized access.")
            => Fail(message, 401);

        public static ApiResponse<object> Forbidden(string message = "You do not have permission to perform this action.")
            => Fail(message, 403);

        public static ApiResponse<object> ServerError(string message = "An unexpected error occurred.")
            => Fail(message, 500);

        public static ApiResponse<object> ValidationError(List<string> errors, string message = "One or more validation errors occurred.")
            => Fail(message, 422, errors);
    }


    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse OkNoData(string message = "Request completed successfully.")
            => new() { Success = true, Message = message, Data = null, StatusCode = 200 };
    }
}