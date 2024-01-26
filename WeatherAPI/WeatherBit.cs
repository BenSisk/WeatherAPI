using Microsoft.AspNetCore.Authorization.Infrastructure;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace WeatherAPI
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
        public WeatherAPIData DecodeJSON(JObject data, string TempUnit)
        {
            try
            {
                if (data != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

                    double Temp;
                    if (TempUnit == "k") { Temp = (double)data["data"][0]["temp"] + 273.15; TempUnit = "K"; }               // Kelvin
                    else if (TempUnit == "f") { Temp = 32 + ((double)data["data"][0]["temp"] / 0.5556); TempUnit = "F"; }   // Fahrenheit
                    else { Temp = (double)data["data"][0]["temp"]; TempUnit = "C"; }                                        // Celsius


                    WeatherAPIData WeatherData = new WeatherAPIData
                    {
                        Temp = Temp,

                        Humidity = (double)data["data"][0]["dhi"],
                        WindSpeed = (double)data["data"][0]["wind_spd"],
                        WindDirection = (double)data["data"][0]["wind_dir"],
                        Pressure = (double)data["data"][0]["pres"],
                        CloudCover = (double)data["data"][0]["clouds"],

                        Longitude = (double)data["data"][0]["lon"],
                        Latitude = (double)data["data"][0]["lat"],
                        TempUnit = TempUnit,

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.

                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
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