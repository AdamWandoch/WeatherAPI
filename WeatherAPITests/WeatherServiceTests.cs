using Domain.DTO;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;

namespace WeatherAPITests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task Get7DaysForecast_InvalidAddressState_ReturnsFailedValidation()
        {
            #region Arrange

            // Setup an invalid address

            var invalidAddress = new AddressDTO()
            {
                Number = 608,
                Street = "Montana Ave",
                City = "Santa Monica",
                State = "CAxx",
                ZipCode = 90403
            };

            var addressValidationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("State", "State should consist of 2 letters only (state abbreviation). For example: AL for Alabama state.")
            });

            var addressValidatorMock = new Mock<IValidator<AddressDTO>>();
            addressValidatorMock.Setup(v => v.Validate(It.IsAny<AddressDTO>())).Returns(addressValidationResult);

            var coordsValidatorMock = new Mock<IValidator<CoordinatesDTO>>();
            var urlValidatorMock = new Mock<IValidator<string>>();
            var forecastValidatorMock = new Mock<IValidator<WeatherForecastDTO>>();

            var configMock = new Mock<IConfiguration>();

            var service = new WeatherService(addressValidatorMock.Object,
                                             coordsValidatorMock.Object,
                                             urlValidatorMock.Object,
                                             forecastValidatorMock.Object,
                                             configMock.Object);

            #endregion

            #region Act

            var result = await service.Get7DaysForecast(invalidAddress);

            #endregion

            #region Assert

            Assert.True(result.Errors.Count == 1);
            Assert.True(result.StatusCode == (int)HttpStatusCode.UnprocessableEntity);
            Assert.True(result.Errors["State"].GetValue(0)?.ToString() == "State should consist of 2 letters only (state abbreviation). For example: AL for Alabama state.");

            #endregion
        }

        [Fact]
        public async Task Get7DaysForecast_InvalidAddressCity_ReturnsFailedValidation()
        {
            #region Arrange

            // Setup an invalid address

            var invalidAddress = new AddressDTO()
            {
                Number = 608,
                Street = "Montana Ave",
                City = "  ",
                State = "CA",
                ZipCode = 90403
            };

            var addressValidationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("City", "'City' should not be empty")
            });

            var addressValidatorMock = new Mock<IValidator<AddressDTO>>();
            addressValidatorMock.Setup(v => v.Validate(It.IsAny<AddressDTO>())).Returns(addressValidationResult);

            var coordsValidatorMock = new Mock<IValidator<CoordinatesDTO>>();
            var urlValidatorMock = new Mock<IValidator<string>>();
            var forecastValidatorMock = new Mock<IValidator<WeatherForecastDTO>>();

            var configMock = new Mock<IConfiguration>();
            //configMock.Setup(c => c["GeocodingApiURL"]).Returns("https://geocoding.geo.census.gov");
            //configMock.Setup(c => c["NationalWeatherApi"]).Returns("https://api.weather.gov");

            var service = new WeatherService(addressValidatorMock.Object,
                                             coordsValidatorMock.Object,
                                             urlValidatorMock.Object,
                                             forecastValidatorMock.Object,
                                             configMock.Object);

            #endregion

            #region Act

            var result = await service.Get7DaysForecast(invalidAddress);

            #endregion

            #region Assert

            Assert.True(result.Errors.Count == 1);
            Assert.True(result.StatusCode == (int)HttpStatusCode.UnprocessableEntity);
            Assert.True(result.Errors["City"].GetValue(0)?.ToString() == "'City' should not be empty");

            #endregion
        }

        [Fact]
        public async Task Get7DaysForecast_GetsInvalidCoordinates()
        {
            #region Arrange

            // Setup a seemingly valid address

            var address = new AddressDTO()
            {
                Number = 1,
                Street = "Montana Ave",
                City = "Santa Monica",
                State = "CA",
                ZipCode = 90403
            };

            var addressValidationResult = new ValidationResult(new List<ValidationFailure> { });
            var addressValidatorMock = new Mock<IValidator<AddressDTO>>();
            addressValidatorMock.Setup(v => v.Validate(It.IsAny<AddressDTO>())).Returns(addressValidationResult);

            // Setup invalid coords

            var invalidCoords = new CoordinatesDTO()
            {
                Latitude = 180,
                Longtitude = -200
            };

            var coordsValidationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Latitude","Invalid"),
                new ValidationFailure("Longtitude","Invalid")
            });
            var coordsValidatorMock = new Mock<IValidator<CoordinatesDTO>>();
            coordsValidatorMock.Setup(v => v.Validate(It.IsAny<CoordinatesDTO>())).Returns(coordsValidationResult);

            var urlValidatorMock = new Mock<IValidator<string>>();
            var forecastValidatorMock = new Mock<IValidator<WeatherForecastDTO>>();

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["GeocodingApiURL"]).Returns("https://geocoding.geo.census.gov");
            configMock.Setup(c => c["NationalWeatherApi"]).Returns("https://api.weather.gov");

            var service = new WeatherService(addressValidatorMock.Object,
                                             coordsValidatorMock.Object,
                                             urlValidatorMock.Object,
                                             forecastValidatorMock.Object,
                                             configMock.Object);

            #endregion

            #region Act

            var result = await service.Get7DaysForecast(address);

            #endregion

            #region Assert

            Assert.True(result.Errors.Count == 2);
            Assert.True(result.StatusCode == (int)HttpStatusCode.UnprocessableEntity);
            Assert.True(result.Errors["Latitude"].GetValue(0)?.ToString() == "Invalid");
            Assert.True(result.Errors["Longtitude"].GetValue(0)?.ToString() == "Invalid");

            #endregion
        }

    }
}