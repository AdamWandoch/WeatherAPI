using Domain.DTO;

namespace Application.Interfaces;

public interface IWeatherService
{
    Task<ResponseDTO<WeatherForecastDTO>> Get7DaysForecast(AddressDTO addressDTO);
}
