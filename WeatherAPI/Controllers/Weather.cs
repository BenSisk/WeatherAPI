using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WeatherAPI.Controllers
{
    [ApiController]
    // Token Bucket Rate Limiting
    [EnableRateLimiting("WeatherAPI")]
    [Route("[controller]")]
    public class Weather : ControllerBase
    {
        public DateOnly Date { get; set; }

        [HttpGet()]

        public async Task<WeatherAPIData> Get(double? Long, double? Lat, string? TempUnit, string? StartDate, string? EndDate)
        {

            await ExternalWeatherAPI.RunAsync();

            return new WeatherAPIData
            {
                Date = DateOnly.FromDateTime(DateTime.Now),

                // if null, default to london
                Longitude = Long is not null ? (double)Long : 51.5072,
                Latitude = Lat is not null ? (double)Lat : 0.1276,

                TempUnit = TempUnit == "C" || TempUnit == "F" || TempUnit == "K" ? TempUnit : "C",
                Temp = 5,
                Humidity = 10,
                WindSpeed = 15,
                WindDirection = 20,
                Precipitation = 25,
                Pressure = 30,
                CloudCover = 35,



                StartDate = StartDate is not null ? DateTime.Parse(StartDate) : DateTime.Now,
                EndDate = EndDate is not null ? DateTime.Parse(EndDate) : DateTime.Now
            };
        }
    }
}