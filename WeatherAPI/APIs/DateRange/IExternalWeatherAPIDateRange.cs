using Newtonsoft.Json.Linq;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.DateRange
{
    // plan to have generic external api class with child classes for different apis
    // testing with just open weather map for now

    public interface IExternalWeatherAPIDateRange
    {
        // Query is the main method to get weather data from the api
        // static abstract Task<WeatherAPIData> Query(double Long = 0.1276, double Lat = 51.5072, string UnitSystem = "metric");

        // returns HTTPClient for the corresponding API chosen
        public abstract HttpClient GetClient();

        //returns instantiated object of the corresponding API to query GetClient() from
        public static abstract IExternalWeatherAPIDateRange GetInstance();

        public WeatherDateRangeData DecodeJSON(JObject data, string TempUnit, DateTime StartDate, DateTime EndDate, double Long, double Lat);

        public abstract string GetURI(double Long, double Lat, DateTime StartDate, DateTime EndDate);
    }
}