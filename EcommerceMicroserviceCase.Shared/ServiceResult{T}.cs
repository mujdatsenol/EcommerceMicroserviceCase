using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace EcommerceMicroserviceCase.Shared;

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    [JsonIgnore]
    public string? UrlAsCreated { get; set; }

    #region Helpers

    // HTTP 200 - OK
    public static ServiceResult<T> SuccessAsOk(T data)
    {
        return new ServiceResult<T>
        {
            Status = HttpStatusCode.OK,
            Data = data
        };
    }
    
    // HTTP 201 - Created
    public static ServiceResult<T> SuccessAsCreated(T data, string url)
    {
        return new ServiceResult<T>
        {
            Status = HttpStatusCode.Created,
            Data = data,
            UrlAsCreated = url
        };
    }

    public new static ServiceResult<T> Error(ProblemDetails problemDetails,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult<T>
        {
            Status = statusCode,
            Fail = problemDetails
        };
    }
    
    public new static ServiceResult<T> Error(string title,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult<T>
        {
            Status = statusCode,
            Fail = new ProblemDetails()
            {
                Title = title,
                Status = statusCode.GetHashCode()
            }
        };
    }

    public new static ServiceResult<T> Error(string title, string description,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        return new ServiceResult<T>
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
    
    public new static ServiceResult<T> ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult<T>
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
    
    public new static ServiceResult<T> ErrorFromProblemDetails(ApiException exception)
    {
        return new ServiceResult<T>()
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