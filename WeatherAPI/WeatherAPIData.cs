using System.ComponentModel;

namespace WeatherAPI
{
    public class WeatherAPIData
    {
        public DateOnly Date { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? TempUnit { get; set; }
        public double Temp { get; set; }
        /*public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);*/
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double Precipitation { get; set; }
        public double Pressure { get; set; }
        public double CloudCover { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}