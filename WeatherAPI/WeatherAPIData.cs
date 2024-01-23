namespace WeatherAPI
{
    public class WeatherAPIData
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        /*public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);*/
        public  string? Unit { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
