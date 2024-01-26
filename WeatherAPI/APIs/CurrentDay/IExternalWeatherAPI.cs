using Newtonsoft.Json.Linq;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.CurrentDay
{
    // plan to have generic external api class with child classes for different apis
    // testing with just open weather map for now

    public interface IExternalWeatherAPI
    {
        // Query is the main method to get weather data from the api
        // static abstract Task<WeatherAPIData> Query(double Long = 0.1276, double Lat = 51.5072, string UnitSystem = "metric");

        // returns HTTPClient for the corresponding API chosen
        public abstract HttpClient GetClient();

        //returns instantiated object of the corresponding API to query GetClient() from
        public static abstract IExternalWeatherAPI GetInstance();

        public WeatherAPIData DecodeJSON(JObject data, string TempUnit);

        public abstract string GetURI(double Long, double Lat);
    }
}