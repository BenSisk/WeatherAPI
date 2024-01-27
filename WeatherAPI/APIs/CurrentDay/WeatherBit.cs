using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.CurrentDay
{
    class WeatherBit : WeatherAPIParent, IExternalWeatherAPI
    {
        private HttpClient? Client;
        private static WeatherBit? instance;

        public HttpClient GetClient()
        {
            if (Client is null) {
                Client = new HttpClient();
                Client.BaseAddress = new Uri("https://api.weatherbit.io/");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return Client;
        }

        public string GetURI(double Long, double Lat)
        {
            return $"/v2.0/current?lat={Lat}&lon={Long}&key={config["WeatherBitAPIKey"]}&units=M";
        }

        // returns an instantiated singleton object of the class for use in the parent class' generic method
        public static IExternalWeatherAPI GetInstance()
        {
            if (instance == null)
            {
                instance = new WeatherBit();
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
                    double Temp = (double)Data["data"][0]["temp"];

                    if (TempUnit == "k") { Temp += 273.15; TempUnit = "K"; }                    // Kelvin
                    else if (TempUnit == "f") { Temp = 32 + (Temp / 0.5556); TempUnit = "F"; }  // Fahrenheit
                    else { TempUnit = "C"; }                                                    // Celsius


                    WeatherAPIData WeatherData = new WeatherAPIData
                    {
                        Date = DateTime.Now,
                        Temp = Temp,

                        Humidity = (double)Data["data"][0]["dhi"],
                        WindSpeed = (double)Data["data"][0]["wind_spd"],
                        WindDirection = (double)Data["data"][0]["wind_dir"],
                        Pressure = (double)Data["data"][0]["pres"],
                        CloudCover = (double)Data["data"][0]["clouds"],

                        Longitude = (double)Data["data"][0]["lon"],
                        Latitude = (double)Data["data"][0]["lat"],
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