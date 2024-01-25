using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{


    public MyClass()
    {

    }

    public class MyClass
    {
        public string MyProperty { get; set; }


    }
}