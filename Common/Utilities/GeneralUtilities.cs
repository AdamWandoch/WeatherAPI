namespace Common.Utilities;

public static class RegexPatterns
{
    public const string URL = @"^(https?|http)://[^\s/$.?#].[^\s]*$";
}

public static class StringCollections
{
    public static readonly List<string> WindDirections = new()
    {   "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
        "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
    };
}
