using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    public class WeatherForecastController : BaseController
    {
        public WeatherForecastController(ILogger<WeatherForecastController> logger) : base(logger) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] AddressDTO addressDTO)
        {
            try
            {
                return new OkObjectResult(await new WeatherService(addressDTO));
            }
            catch (Exception ex)
            {
                return RespondToException(ex);
            }
        }
    }
}