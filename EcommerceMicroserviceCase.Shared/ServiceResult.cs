using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace EcommerceMicroserviceCase.Shared;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

public class ServiceResult
{
    [JsonIgnore]
    public HttpStatusCode Status { get; set; }

    public ProblemDetails? Fail { get; set; }
    
    [JsonIgnore]
    public bool Success => Fail is null;
    
    [JsonIgnore]
    public bool IsFail => !Success;

    #region Helpers

    // HTTP 204 - NoContent
    public static ServiceResult SuccessAsNoContent()
    {
        return new ServiceResult { Status = HttpStatusCode.NoContent };
    }
    
    // HTTP 404 - NotFound
    public static ServiceResult ErrorAsNotFound()
    {
        return new ServiceResult
        {
            Status = HttpStatusCode.NotFound,
            Fail = new ProblemDetails
            {
                Title = "Not Found",
                Detail = "The requested resource was not found."
            }
        };
    }
    
    public static ServiceResult Error(ProblemDetails problemDetails,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult
        {
            Status = statusCode,
            Fail = problemDetails
        };
    }
    
    public static ServiceResult Error(string title,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult
        {
            Status = statusCode,
            Fail = new ProblemDetails()
            {
                Title = title,
                Status = statusCode.GetHashCode()
            }
        };
    }

    public static ServiceResult Error(string title, string description,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult
        {
            Status = statusCode,
            Fail = new ProblemDetails()
            {
                Title = title,
                Detail = description,
                Status = statusCode.GetHashCode()
            }
        };
    }
    
    public static ServiceResult ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult
        {
            Status = HttpStatusCode.BadRequest,
            Fail = new ProblemDetails()
            {
                Title = "Validation errors occurred",
                Detail = "Please check the errors property for more details.",
                Status = HttpStatusCode.BadRequest.GetHashCode(),
                Extensions = errors
            }
        };
    }
    
    public static ServiceResult ErrorFromProblemDetails(ApiException exception)
    {
        return new ServiceResult()
        {
            Status = exception.StatusCode,
            Fail = string.IsNullOrEmpty(exception.Content)
                ? new ProblemDetails { Title = exception.Message }
                : JsonSerializer.Deserialize<ProblemDetails>(exception.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
        };
    }

    #endregion
}