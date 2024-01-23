using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [EnableRateLimiting("WeatherAPI")]
    [Route("[controller]")]
    public class Weather : ControllerBase
    {
        public DateOnly Date { get; set; }

        [HttpGet()]

        public WeatherAPIData Get(double? Long, double? Lat, string? Unit, string? StartDate, string? EndDate)
        {
            return new WeatherAPIData
            {
                Date = DateOnly.FromDateTime(DateTime.Now),

                TemperatureC = 5,

                // if null, default to london
                Longitude = Long is not null ? (double)Long : 51.5072,
                Latitude = Lat is not null ? (double)Lat : 0.1276,

                Unit = Unit == "C" || Unit == "F" || Unit == "K" ? Unit : "C",

                StartDate = StartDate is not null ? DateTime.Parse(StartDate) : DateTime.Now,
                EndDate = EndDate is not null ? DateTime.Parse(EndDate) : DateTime.Now
            };
        }
    }
}