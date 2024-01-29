using Application;
using Application.Authentication;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
string allowedHost = builder.Configuration["ClientHostName"];

// Add services to the container.

builder.Services.AddAuthentication(options =>
{
    // Define the authentication scheme
    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
})
.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options => { });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddControllers();

// DI
builder.Services.AddApplication().AddInfrastructure();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins(allowedHost)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Configure Kestrel to listen on HTTPS only
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5001, listenOptions => // HTTPS port
//    {
//        listenOptions.UseHttps();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
