using Domain.DTO;
using FluentValidation.Results;
using System.Net;
namespace Infrastructure.Services;

public class BaseService
{
    public static HttpClient GetHttpClient(string baseAddress)
    {
        var httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherAPIUpstart13AW/1.0");

        return httpClient;
    }

    public static string BuildQueryString(Dictionary<string, string> queryParams)
    {
        var queryString = new System.Text.StringBuilder("?");
        foreach (var param in queryParams)
        {
            queryString.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}&");
        }

        return queryString.ToString();
    }

    public static ResponseDTO<T> FailedValidation<T>(ResponseDTO<T> result, ValidationResult addressValidation) where T : class
    {
        result.Errors = addressValidation.ToDictionary();
        result.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
        return result;
    }
}
