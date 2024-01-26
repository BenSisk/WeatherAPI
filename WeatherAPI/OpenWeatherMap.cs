using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace WeatherAPI
{
    public class OpenWeatherMap : IExternalWeatherAPI
    {
        private static HttpClient? Client;

        private static HttpClient GetClient()
        {
            if (Client is null) {
                Client = new HttpClient();
                Client.BaseAddress = new Uri("https://api.openweathermap.org:443/");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return Client;
        }

        private static IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();


        private static async Task<JObject> GetWeatherAsync(string path)
        {
            HttpResponseMessage response = await GetClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Console.WriteLine(data);
                JObject json = JObject.Parse(data);
                return json;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }


        public static async Task<WeatherAPIData> Query(double Long = 0.1276, double Lat = 51.5072, string UnitSystem = "metric")
        {
            try
            {
                var data = await GetWeatherAsync($"/data/2.5/weather?lat={Lat}&lon={Long}&appid={config["OpenWeatherMapAPI"]}&units={UnitSystem}");

                WeatherAPIData WeatherData = new WeatherAPIData
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),

                    Temp = (double)data["main"]["temp"],
                    Humidity = (double)data["main"]["humidity"],
                    WindSpeed = (double)data["wind"]["speed"],
                    WindDirection = (double)data["wind"]["deg"],
                    Pressure = (double)data["main"]["pressure"],
                    CloudCover = (double)data["clouds"]["all"],

                    Longitude = Long,
                    Latitude = Lat,
                    UnitSystem = UnitSystem,

                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                };

                return WeatherData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}