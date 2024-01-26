using System.ComponentModel;

namespace WeatherAPI.DataStructs
{
    public class WeatherAPIData
    {
        public required DateTime Date { get; set; }
        public required double Longitude { get; set; }
        public required double Latitude { get; set; }
        public required string TempUnit { get; set; }
        public required double Temp { get; set; }
        public required double Humidity { get; set; }
        public required double WindSpeed { get; set; }
        public required double WindDirection { get; set; }
        public required double Pressure { get; set; }
        public required double CloudCover { get; set; }
    }
}