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
                Client.BaseAddress = new Uri("https://api.open-meteo.com/");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return Client;
        }

        public string GetURI(double Long, double Lat, DateTime StartDate, DateTime EndDate)
        {
            return $"https://archive-api.open-meteo.com/v1/archive?latitude={Lat}&longitude={Long}" +
                $"&start_date={StartDate.ToString("yyyy-MM-dd")}&end_date={EndDate.ToString("yyyy-MM-dd")}&hourly=temperature_2m&wind_speed_unit=ms";
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
                    List<double> DataList = Data["hourly"]["temperature_2m"].Select(x => (double)x).ToList();

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.

                    double MinTemp = DataList.Min();
                    double MaxTemp = DataList.Max();
                    double AvgTemp = Math.Round(DataList.Average(), 2);

                    // Default Celsius
                    if (TempUnit == "k"){ MinTemp += 273.15; MaxTemp += 273.15; AvgTemp += 273.15; TempUnit = "K"; }                                                        // Kelvin
                    else if (TempUnit == "f") { MinTemp = 32 + (MinTemp / 0.5556); MaxTemp = 32 + (MaxTemp / 0.5556); AvgTemp = 32 + (AvgTemp / 0.5556);  TempUnit = "F"; } // Fahrenheit
                    else { TempUnit = "C"; }

                    WeatherDateRangeData WeatherData = new WeatherDateRangeData
                    {
                        StartDate = StartDate,
                        EndDate = EndDate,
                        Longitude = Long,
                        Latitude = Lat,

                        TempUnit = TempUnit,

                        MinTemp = MinTemp,
                        MaxTemp = MaxTemp,
                        AvgTemp = AvgTemp
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