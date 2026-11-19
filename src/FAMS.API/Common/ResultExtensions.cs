using FAMS.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FAMS.API.Common;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse<T>.SuccessResponse(result.Value));
        }

        return CreateErrorResult<T>(result.Error);
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse<object>.SuccessResponse(new { }));
        }

        return CreateErrorResult<object>(result.Error);
    }

    private static IActionResult CreateErrorResult<T>(Error error)
    {
        var response = ApiResponse<T>.FailureResponse(error.Message);

        return error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(response),
            ErrorType.Unauthorized => new ObjectResult(response) { StatusCode = 401 },
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Conflict => new ConflictObjectResult(response),
            _ => new ObjectResult(response) { StatusCode = 500 }
        };
    }
}