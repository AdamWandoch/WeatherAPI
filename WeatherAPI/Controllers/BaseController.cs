using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public BaseController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    public IActionResult RespondToException(Exception ex)
    {
        return new BadRequestObjectResult(new ExceptionMessage() { Message = ex.Message, StackTrace = ex.StackTrace });
    }

    public class ExceptionMessage
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}