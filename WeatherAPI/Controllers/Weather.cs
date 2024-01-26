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
        public async Task<WeatherAPIData> Get(double? Long, double? Lat, string? TempUnit, string? StartDate, string? EndDate, string? API)
        {

            // if null, default to london
            double Longitude = Long is not null ? (double)Long : -0.11489;
            double Latitude = Lat is not null ? (double)Lat : 51.51418;

            TempUnit = TempUnit is not null ? TempUnit.ToLower() : "C";
            TempUnit = TempUnit == "c" || TempUnit == "f"  || TempUnit == "k" ? TempUnit : "c";


            // default to OpenWeatherMap, weatherbit is an optional variable
            IExternalWeatherAPI ChosenAPI;
            if (API is not null && API.ToLower() == "weatherbit") ChosenAPI = WeatherBit.GetInstance();
            else ChosenAPI = OpenWeatherMap.GetInstance();

            WeatherAPIData data = await WeatherAPIParent.Query(ChosenAPI, Longitude, Latitude, TempUnit);

            return data;
        }
    }
}