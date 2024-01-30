using Domain.DTO;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace WeatherAPITests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task Get7DaysForecast_InvalidAddress_ReturnsFailedValidation()
        {
            #region Arrange

            var addressValidatorMock = new Mock<IValidator<AddressDTO>>();
            var invalidAddres = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("State", "State should consist of 2 letters only (state abbreviation). For example: AL for Alabama state.")
            });
            addressValidatorMock.Setup(v => v.Validate(It.IsAny<AddressDTO>())).Returns(invalidAddres);

            var coordsValidatorMock = new Mock<IValidator<CoordinatesDTO>>();
            var urlValidatorMock = new Mock<IValidator<string>>();
            var forecastValidatorMock = new Mock<IValidator<WeatherForecastDTO>>();
            var configMock = new Mock<IConfiguration>();


            var service = new WeatherService(addressValidatorMock.Object,
                                             coordsValidatorMock.Object,
                                             urlValidatorMock.Object,
                                             forecastValidatorMock.Object,
                                             configMock.Object);

            var addressDTO = new AddressDTO() // Setup an invalid address
            {
                Number = 608,
                Street = "Montana Ave",
                City = "Santa Monica",
                State = "CAxx",
                ZipCode = 90403
            };

            #endregion

            #region Act

            var result = await service.Get7DaysForecast(addressDTO);

            #endregion

            #region Assert

            Assert.False(result.Errors.Count == 0);

            #endregion
        }

    }
}