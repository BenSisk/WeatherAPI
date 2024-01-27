using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.CurrentDay
{
    class OpenWeatherMap : WeatherAPIParent, IExternalWeatherAPI
    {
        private HttpClient? Client;
        private static OpenWeatherMap? instance;

        public HttpClient GetClient()
        {
            if (Client is null)
            {
                Client = new HttpClient();
                Client.BaseAddress = new Uri("https://api.openweathermap.org");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return Client;
        }

        public string GetURI(double Long, double Lat)
        {
            return $"/data/2.5/weather?lat={Lat}&lon={Long}&appid={config["OpenWeatherMapAPIKey"]}&units=metric";
        }

        // returns an instantiated singleton object of the class for use in the parent class' generic method
        public static IExternalWeatherAPI GetInstance()
        {
            if (instance == null)
            {
                instance = new OpenWeatherMap();
            }
            return instance;
        }


        // Decodes JSON into a WeatherAPI data object using the API's schema
        public WeatherAPIData DecodeJSON(JObject Data, string TempUnit)
        {
            try
            {
                if (Data != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

                    // Default Celsius
                    double Temp = (double)Data["main"]["temp"];

                    if (TempUnit == "k") { Temp += 273.15; TempUnit = "K"; }               // Kelvin
                    else if (TempUnit == "f") { Temp = 32 + (Temp / 0.5556); TempUnit = "F"; }   // Fahrenheit


                    WeatherAPIData WeatherData = new WeatherAPIData
                    {
                        Date = DateTime.Now,
                        Temp = Temp,
                        Humidity = (double)Data["main"]["humidity"],
                        WindSpeed = (double)Data["wind"]["speed"],
                        WindDirection = (double)Data["wind"]["deg"],
                        Pressure = (double)Data["main"]["pressure"],
                        CloudCover = (double)Data["clouds"]["all"],

                        Longitude = (double)Data["coord"]["lon"],
                        Latitude = (double)Data["coord"]["lat"],
                        TempUnit = TempUnit,

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
                    };

                    return WeatherData;
                }
                throw new Exception("No data found, value is null");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}