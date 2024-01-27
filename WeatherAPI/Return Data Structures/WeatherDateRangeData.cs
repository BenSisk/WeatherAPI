namespace WeatherAPI.DataStructs
{
    public class WeatherDateRangeData
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required double Longitude { get; set; }
        public required double Latitude { get; set; }
        public required string? TempUnit { get; set; }
        public required double MinTemp { get; set; }
        public required double MaxTemp { get; set; }
        public required double AvgTemp { get; set; }
    }
}