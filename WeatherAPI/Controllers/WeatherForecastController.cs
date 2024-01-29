using Application.Interfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    public class WeatherForecastController : BaseController<WeatherForecastController>
    {
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService) : base(logger)
        {
            _weatherService = weatherService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<WeatherForecastDTO>>> Get([FromQuery] AddressDTO addressDTO)
        {
            try
            {
                var result = await _weatherService.Get7DaysForecast(addressDTO);
                return new ObjectResult(result) { StatusCode = result.StatusCode };
            }
            catch (Exception ex)
            {
                var exceptionResponse = RespondToException<WeatherForecastDTO>(ex);
                return new ObjectResult(exceptionResponse) { StatusCode = exceptionResponse.StatusCode };
            }
        }
    }
}