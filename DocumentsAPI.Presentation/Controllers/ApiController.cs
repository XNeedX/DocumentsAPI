using Microsoft.AspNetCore.Mvc;
using Services.Application.Results;
using Profiles.Presentation.Responses;

namespace Profiles.Presentation.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result, string? successMessage = null) =>
        result.IsSuccess
            ? Ok(ApiResponse<T>.Success(result.Value, successMessage))
            : HandleFailure(result); 

    protected IActionResult HandleResult(Result result, string? successMessage = null) =>
        result.IsSuccess
            ? Ok(ApiResponse.Success(successMessage))
            : HandleFailure(result); 

    protected IActionResult HandleCreatedResult<T>(Result<T> result, string? successMessage = null) =>
        result.IsSuccess
            ? StatusCode(201, ApiResponse<T>.Success(result.Value, successMessage))
            : HandleFailure(result); 

    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),

            IValidationResult validationResult =>
                BadRequest(ApiResponse.Failure(
                    validationResult.Errors.Select(e => e.Message).ToList(),
                    "Validation falure")),

            _ => result.Error.Type switch
            {
                ErrorType.NotFound => NotFound(ApiResponse.Failure(result.Error.Message)),
                ErrorType.Conflict => Conflict(ApiResponse.Failure(result.Error.Message)),
                ErrorType.Forbidden => StatusCode(403, ApiResponse.Failure(result.Error.Message)),
                ErrorType.Validation => BadRequest(ApiResponse.Failure(result.Error.Message)),
                _ => BadRequest(ApiResponse.Failure(result.Error.Message))
            }
        };
}