using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Application.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    public string Scheme => DefaultScheme;
    public string AuthenticationType = DefaultScheme;
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IConfiguration _configuration;
    private const string API_KEY_NAME = "WEATHER-API-KEY";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration) : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Get the API key from the request header
        if (!Request.Headers.TryGetValue(API_KEY_NAME, out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        var isValidApiKey = providedApiKey.Equals(_configuration[$"{API_KEY_NAME}"]);

        if (!isValidApiKey)
        {
            return AuthenticateResult.Fail("Invalid API Key provided.");
        }

        // Create the authentication ticket (claims) based on the API key
        var claims = new[] { new Claim(ClaimTypes.Name, "WeatherAPIClient") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}