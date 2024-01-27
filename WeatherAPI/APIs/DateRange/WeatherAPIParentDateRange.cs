using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.DateRange
{
    public class WeatherAPIParentDateRange
    {
        protected static IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();


        // Attempted to use a generic method to get weather data from any api
        // not 100% sure it will work asynchronusly due to getting an instance of the object

        // I think it works in theory as the object is instatiated if null, and only read from not written to after this
        // uses dependency injection on api using the interface IExternalWeatherAPI
        private static async Task<JObject> GetWeatherAsync(string path, IExternalWeatherAPIDateRange API)
        {
            Console.WriteLine(path);
            HttpResponseMessage response = await API.GetClient().GetAsync(path);
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                string Data = await response.Content.ReadAsStringAsync();
                JObject JSON = JObject.Parse(Data);
                return JSON;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task<WeatherDateRangeData> Query(IExternalWeatherAPIDateRange API, DateTime StartDate, DateTime EndDate, double Long = 0.1276, double Lat = 51.5072, string TempUnit = "C")
        {
            try
            {
                // grabs the specific URI from the corresponding API class with its user secrets API key,
                // inserts the lat and long into the URI, and passes it to GetWeatherAsync to query it
                JObject JSONData = await GetWeatherAsync(API.GetURI(Long, Lat, StartDate, EndDate), API);

                // sends the result back to the API class to decode the JSON into a WeatherAPIData object, and returns it to the controller
                return API.DecodeJSON(JSONData, TempUnit, StartDate, EndDate, Long, Lat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}