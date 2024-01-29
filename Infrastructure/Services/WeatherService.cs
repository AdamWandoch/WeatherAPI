using Application.Interfaces;
using Domain.DTO;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class WeatherService : BaseService, IWeatherService
{
    private readonly IValidator<AddressDTO> _addressValidator;
    private readonly IValidator<CoordinatesDTO> _coordsValidator;
    private readonly IValidator<string> _urlValidator;
    private readonly IValidator<WeatherForecastDTO> _weatherForecastValidator;
    private readonly IConfiguration _configuration;

    public WeatherService(IValidator<AddressDTO> addressValidator,
                          IValidator<CoordinatesDTO> coordsValidator,
                          IValidator<string> urlValidator,
                          IValidator<WeatherForecastDTO> weatherForecastValidator,
                          IConfiguration configuration)
    {
        _addressValidator = addressValidator;
        _coordsValidator = coordsValidator;
        _urlValidator = urlValidator;
        _weatherForecastValidator = weatherForecastValidator;
        _configuration = configuration;
    }

    public async Task<ResponseDTO<WeatherForecastDTO>> Get7DaysForecast(AddressDTO addressDTO)
    {
        var result = new ResponseDTO<WeatherForecastDTO>();

        var addressValidation = _addressValidator.Validate(addressDTO);
        if (!addressValidation.IsValid)
        {
            return FailedValidation(result, addressValidation);
        }

        var coordsDTO = await GetCoordinates(addressDTO);
        var coordsValidation = _coordsValidator.Validate(coordsDTO);
        if (!coordsValidation.IsValid)
        {
            return FailedValidation(result, coordsValidation);
        }

        var forecastResultURL = await GetForecastResultURL(coordsDTO);
        var urlValidation = _urlValidator.Validate(forecastResultURL);
        if (!urlValidation.IsValid)
        {
            return FailedValidation(result, urlValidation);
        }

        var forecastDetails = await GetForecastDetails(forecastResultURL);
        var forecastValidation = _weatherForecastValidator.Validate(forecastDetails);
        if (!forecastValidation.IsValid)
        {
            return FailedValidation(result, forecastValidation);
        }

        result.ResponseData = forecastDetails;
        return result;
    }

    private async Task<CoordinatesDTO> GetCoordinates(AddressDTO addressDTO)
    {
        using var httpClient = GetHttpClient(_configuration["GeocodingApiURL"]);

        var queryParams = new Dictionary<string, string>
        {
            {"address", $"{addressDTO.Number} {addressDTO.Street}, {addressDTO.City}, {addressDTO.State} {addressDTO.ZipCode}"},
            {"benchmark", "Public_AR_Current"},
            {"format", "json"}
        };

        var queryString = BuildQueryString(queryParams);

        try
        {
            var response = await httpClient.GetAsync("/geocoder/locations/onelineaddress" + queryString);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<dynamic>(jsonString);
                var addressMatches = deserialized?.result.addressMatches;

                if (addressMatches is null) return new CoordinatesDTO();
                if (((JArray)addressMatches).Count == 0) return new CoordinatesDTO();

                return new CoordinatesDTO()
                {
                    Longtitude = addressMatches[0].coordinates.x,
                    Latitude = addressMatches[0].coordinates.y
                };
            }
            else
            {
                throw new ServiceException($"Coordinates reponse error: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new ServiceException("A problem has occurred while retrieving coordinates.", ex);
        }
    }

    private async Task<string> GetForecastResultURL(CoordinatesDTO coordsDTO)
    {
        using var httpClient = GetHttpClient(_configuration["NationalWeatherApi"]);
        httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/ld+json");

        try
        {
            var response = await httpClient.GetAsync($"/points/{coordsDTO.Latitude},{coordsDTO.Longtitude}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<dynamic>(jsonString);
                var forecastResultURL = deserialized?.forecast;

                return forecastResultURL ?? string.Empty;
            }
            else
            {
                throw new ServiceException($"Forecast result URL reponse error: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new ServiceException("A problem has occurred while retrieving forecast result URL.", ex);
        }
    }

    private async Task<WeatherForecastDTO> GetForecastDetails(string forecastResultURL)
    {
        using var httpClient = GetHttpClient(_configuration["NationalWeatherApi"]);
        httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/ld+json");

        try
        {
            var response = await httpClient.GetAsync(forecastResultURL.Replace("https://api.weather.gov", ""));

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<WeatherForecastDTO>(jsonString);
                return result ?? new WeatherForecastDTO();
            }
            else
            {
                throw new ServiceException($"Forecast result details reponse error: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new ServiceException("A problem has occurred while retrieving forecast result details.", ex);
        }
    }
}
