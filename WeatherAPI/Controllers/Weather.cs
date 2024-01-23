using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Weather : ControllerBase
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }

        [HttpGet(Name = "GetWeather")]

        public WeatherAPIData Get()
        {
            return new WeatherAPIData
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 5,
                Longitude = 10,
                Latitude = 15,
                Unit = "Celsius"
            };
        }
    }
}