using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WeatherAPI.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController<T> : ControllerBase
{
    private readonly ILogger<T> _logger;

    public BaseController(ILogger<T> logger)
    {
        _logger = logger;
    }

    public ResponseDTO<U> RespondToException<U>(Exception ex) where U : class
    {
        _logger.LogError(ex.Message, ex);
        var errors = new Dictionary<string, string[]>
        {
            { "ExceptionThrown", new string[] { ex.Message } },
            { "InnerExceptionMessage", new string[] { ex.InnerException?.Message ?? "No InnerExceptionMessage available." } },
            { "StackTrace", new string[] { ex.StackTrace ?? "No StackTrace available." } }
        };
        return new ResponseDTO<U>() { StatusCode = (int)HttpStatusCode.InternalServerError, Errors = errors };
    }

    public class ExceptionMessage
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}