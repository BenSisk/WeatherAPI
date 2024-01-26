using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherAPI.APIs.CurrentDay;
using WeatherAPI.APIs.DateRange;
using WeatherAPI.DataStructs;

namespace WeatherAPI.Controllers
{
    [ApiController]
    // Token Bucket Rate Limiting
    [EnableRateLimiting("WeatherAPI")]
    public class Weather : ControllerBase
    {
        public DateOnly Date { get; set; }

        [HttpGet()]
        [Route("[controller]")]
        public async Task<WeatherAPIData> Get(double? Long, double? Lat, string? TempUnit, string? API)
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



        [HttpGet()]
        [Route("weather/date-range&startdate={StartDate}&enddate={EndDate}")]
        public async Task<WeatherDateRangeData> GetDateRange(double? Long, double? Lat, string? TempUnit, DateTime StartDate, DateTime EndDate)
        {

            // if null, default to london
            double Longitude = Long is not null ? (double)Long : -0.11489;
            double Latitude = Lat is not null ? (double)Lat : 51.51418;

            TempUnit = TempUnit is not null ? TempUnit.ToLower() : "C";
            TempUnit = TempUnit == "c" || TempUnit == "f" || TempUnit == "k" ? TempUnit : "c";

            WeatherDateRangeData data = await WeatherAPIParentDateRange.Query(OpenWeatherMapDateRange.GetInstance(), StartDate, EndDate, Longitude, Latitude, TempUnit);

            return data;
        }
    }
}