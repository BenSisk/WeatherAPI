namespace WeatherAPI
{
    // plan to have generic external api class with child classes for different apis
    // testing with just open weather map for now

    public interface IExternalWeatherAPI
    {
        static abstract Task<WeatherAPIData> Query(double Long = 0.1276, double Lat = 51.5072, string UnitSystem = "metric");
    }
}