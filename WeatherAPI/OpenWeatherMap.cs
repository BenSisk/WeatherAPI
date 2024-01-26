﻿using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace WeatherAPI
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
        public WeatherAPIData DecodeJSON(JObject data, string TempUnit)
        {
            try
            {
                if (data != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

                    double Temp;
                    if (TempUnit == "k") { Temp = (double)data["main"]["temp"] + 273.15; TempUnit = "K"; }               // Kelvin
                    else if (TempUnit == "f") { Temp = 32 + ((double)data["main"]["temp"] / 0.5556); TempUnit = "F"; }   // Fahrenheit
                    else { Temp = (double)data["main"]["temp"]; TempUnit = "C"; }                                        // Celsius


                    WeatherAPIData WeatherData = new WeatherAPIData
                    {
                        
                        Temp = Temp,
                        Humidity = (double)data["main"]["humidity"],
                        WindSpeed = (double)data["wind"]["speed"],
                        WindDirection = (double)data["wind"]["deg"],
                        Pressure = (double)data["main"]["pressure"],
                        CloudCover = (double)data["clouds"]["all"],

                        Longitude = (double)data["coord"]["lon"],
                        Latitude = (double)data["coord"]["lat"],
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