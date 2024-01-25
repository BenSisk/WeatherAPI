using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
        public async Task<WeatherAPIData> Get(double? Long, double? Lat, string? UnitSystem, string? StartDate, string? EndDate)
        {

            // if null, default to london

            double Longitude = Long is not null ? (double)Long : 51.5072;
            double Latitude = Lat is not null ? (double)Lat : 0.1276;

            UnitSystem = UnitSystem is not null ? UnitSystem.ToLower() : "metric";
            UnitSystem = UnitSystem == "metric" || UnitSystem == "imperial" ? UnitSystem : "metric";

            WeatherAPIData data = await OpenWeatherMap.Query(Longitude, Latitude, UnitSystem);

            return data;

            /*return new WeatherAPIData
            {
                Date = DateOnly.FromDateTime(DateTime.Now),

                TempUnit = TempUnit == "C" || TempUnit == "F" || TempUnit == "K" ? TempUnit : "C",
                Temp = 0,
                Humidity = 0,
                WindSpeed = 0,
                WindDirection = 0,
                Precipitation = 0,
                Pressure = 0,
                CloudCover = 0,



                StartDate = StartDate is not null ? DateTime.Parse(StartDate) : DateTime.Now,
                EndDate = EndDate is not null ? DateTime.Parse(EndDate) : DateTime.Now
            };*/
        }
    }
}