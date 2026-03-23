using AuthDemo.Common;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    /// <summary>
    /// Base controller that provides ApiResponse helper methods.
    /// All controllers should inherit this instead of ControllerBase.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ApiOk<T>(T data, string message = "Request completed successfully.")
            => Ok(ApiResponse<T>.Ok(data, message));

        protected IActionResult ApiCreated<T>(T data, string message = "Resource created successfully.")
            => StatusCode(201, ApiResponse<T>.Created(data, message));

        protected IActionResult ApiNoContent(string message = "Request completed successfully.")
            => Ok(ApiResponse.OkNoData(message));

        protected IActionResult ApiBadRequest(string message, List<string>? errors = null)
            => BadRequest(ApiResponse<object>.Fail(message, 400, errors));

        protected IActionResult ApiNotFound(string message = "Resource not found.")
            => NotFound(ApiResponse<object>.NotFound(message));

        protected IActionResult ApiForbidden(string message = "Forbidden.")
            => StatusCode(403, ApiResponse<object>.Forbidden(message));

        protected IActionResult ApiValidationError(List<string> errors)
            => StatusCode(422, ApiResponse<object>.ValidationError(errors));

    }
}