using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using WeatherAPI.DataStructs;

namespace WeatherAPI.APIs.DateRange
{
    public class OpenWeatherMapDateRange : WeatherAPIParentDateRange, IExternalWeatherAPIDateRange
    {
        private HttpClient? Client;
        private static OpenWeatherMapDateRange? instance;

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

        public string GetURI(double Long, double Lat, DateTime StartDate, DateTime EndDate)
        {
            return $"/data/2.5/weather?lat={Lat}&lon={Long}&type=hour&units=metric&appid={config["OpenWeatherMapAPIKey"]}" + 
                $"&start={((DateTimeOffset)StartDate).ToUnixTimeSeconds()}&end={((DateTimeOffset)EndDate).ToUnixTimeSeconds()}";
        }

        // returns an instantiated singleton object of the class for use in the parent class' generic method
        public static IExternalWeatherAPIDateRange GetInstance()
        {
            if (instance == null)
            {
                instance = new OpenWeatherMapDateRange();
            }
            return instance;
        }


        // Decodes JSON into a WeatherAPI data object using the API's schema
        public WeatherDateRangeData DecodeJSON(JObject Data, string TempUnit, DateTime StartDate, DateTime EndDate, double Long, double Lat)
        {
            try
            {
                if (Data != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

                    // Default Celsius

                    // this is the incorrect temp min and max, and actually represents the min and max temp at the time of the query.
                    // Leaving as placeholder for now, as I think an actual solution will require querying each day individually
                    // may develop a cache for this so it doesnt gobble all my allowed queries.
                    double MinTemp = (double)Data["main"]["temp_min"];
                    double MaxTemp = (double)Data["main"]["temp_max"];
                    TempUnit = "C";


                    if (TempUnit == "k"){ MinTemp += 273.15; MaxTemp += 273.15; TempUnit = "K"; }                                       // Kelvin
                    else if (TempUnit == "f") { MinTemp = 32 + (MinTemp / 0.5556); MaxTemp = 32 + (MaxTemp / 0.5556); TempUnit = "F"; } // Fahrenheit


                    WeatherDateRangeData WeatherData = new WeatherDateRangeData
                    {
                        StartDate = StartDate,
                        EndDate = EndDate,
                        Longitude = Long,
                        Latitude = Lat,

                        Humidity = (double)Data["main"]["humidity"],
                        WindSpeed = (double)Data["wind"]["speed"],
                        WindDirection = (double)Data["wind"]["deg"],
                        Pressure = (double)Data["main"]["pressure"],
                        CloudCover = (double)Data["clouds"]["all"],

                        TempUnit = TempUnit,
                        WeatherDescription = (string?)Data["weather"][0]["description"],
                        MinTemp = MinTemp,
                        MaxTemp = MaxTemp

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