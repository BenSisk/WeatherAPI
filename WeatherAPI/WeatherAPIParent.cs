using Newtonsoft.Json.Linq;
using System.Drawing;

namespace WeatherAPI
{
    public class WeatherAPIParent
    {
        protected static IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();


        // Attempted to use a generic method to get weather data from any api
        // not 100% sure it will work asynchronusly due to getting an instance of the object

        // I think it works in theory as the object is instatiated if null, and only read from not written to after this
        // uses dependency injection on api using the interface IExternalWeatherAPI
        protected static async Task<JObject> GetWeatherAsync(string path, IExternalWeatherAPI API)
        {
            HttpResponseMessage response = await API.GetClient().GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(data);
                JObject json = JObject.Parse(data);
                return json;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task<WeatherAPIData> Query(IExternalWeatherAPI API, double Long = 0.1276, double Lat = 51.5072, string TempUnit = "C")
        {
            try
            {
                // grabs the specific URI from the corresponding API class with its user secrets API key,
                // inserts the lat and long into the URI, and passes it to GetWeatherAsync to query it
                JObject Data = await GetWeatherAsync( API.GetURI(Long, Lat), API);

                // sends the result back to the API class to decode the JSON into a WeatherAPIData object, and returns it to the controller
                return API.DecodeJSON(Data, TempUnit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
