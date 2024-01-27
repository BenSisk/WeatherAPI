﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get(double? Long, double? Lat, string? TempUnit, string? API)
        {
            // if null, default to london
            double Longitude = Long is not null ? (double)Long : -0.11489;
            double Latitude = Lat is not null ? (double)Lat : 51.51418;

            TempUnit = TempUnit is not null ? TempUnit.ToLower() : "c";

            if (Lat < -90 || Lat > 90 || Long < -180 || Long > 180) return BadRequest("Invalid latitude or longitude");
            if (TempUnit != "c" && TempUnit != "f" && TempUnit != "k") return BadRequest("Invalid temperature unit. Please use 'C', 'K' or 'F'");
            TempUnit = TempUnit == "c" || TempUnit == "f"  || TempUnit == "k" ? TempUnit : "c";


            // default to OpenWeatherMap, weatherbit is an optional variable
            IExternalWeatherAPI ChosenAPI;
            if (API is not null && API.ToLower() == "weatherbit") ChosenAPI = WeatherBit.GetInstance();
            else ChosenAPI = OpenWeatherMap.GetInstance();

            WeatherAPIData data = await WeatherAPIParent.Query(ChosenAPI, Longitude, Latitude, TempUnit);

            if (data is not null) return Ok(data);
            else return NotFound("Error 404: The selected API may be temporarily unavailable");
        }



        [HttpGet()]
        [Route("weather/date-range&startdate={StartDate}&enddate={EndDate}")]
        public async Task<IActionResult> GetDateRange(double? Long, double? Lat, string? TempUnit, DateTime StartDate, DateTime EndDate)
        {
            // if null, default to london
            double Longitude = Long is not null ? (double)Long : -0.11489;
            double Latitude = Lat is not null ? (double)Lat : 51.51418;

            TempUnit = TempUnit is not null ? TempUnit.ToLower() : "c";

            if (Lat < -90 || Lat > 90 || Long < -180 || Long > 180) return BadRequest("Invalid latitude or longitude");
            if (TempUnit != "c" && TempUnit != "f" && TempUnit != "k") return BadRequest("Invalid temperature unit. Please use 'C', 'K' or 'F'");


            TempUnit = TempUnit == "c" || TempUnit == "f" || TempUnit == "k" ? TempUnit : "c";

            WeatherDateRangeData data = await WeatherAPIParentDateRange.Query(OpenWeatherMapDateRange.GetInstance(), StartDate, EndDate, Longitude, Latitude, TempUnit);

            if (data is not null) return Ok(data);
            else return NotFound("Error 404: The selected API may be temporarily unavailable");
        }
    }
}