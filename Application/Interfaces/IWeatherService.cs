using Domain.DTO;

namespace Application.Interfaces;

public interface IWeatherService
{
    /// <summary>
    /// Processes a forecast request
    /// </summary>
    /// <param name="addressDTO">Address for which the forecast was requested</param>
    /// <returns>7 day weather forecast data</returns>
    Task<ResponseDTO<WeatherForecastDTO>> Get7DaysForecast(AddressDTO addressDTO);
}
