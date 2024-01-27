namespace Domain.DTO;

public class WeatherForecastDTO
{
    public List<PeriodDTO>? Periods { get; set; }
}

public class PeriodDTO
{
    public int Number { get; set; }
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDaytime { get; set; }
    public short Temperature { get; set; }
    public char TemperatureUnit { get; set; }
    public HumidityDTO RelativeHumidity { get; set; } = new();
    public string? WindSpeed { get; set; }
    public string WindDirection { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? ShortForecast { get; set; }
    public string? DetailedForecast { get; set; }
}

public class HumidityDTO
{
    public short Value { get; set; }
}