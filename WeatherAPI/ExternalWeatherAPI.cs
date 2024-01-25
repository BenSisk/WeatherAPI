using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherAPI
{
    // plan to have generic external api class with child classes for different apis
    // testing with just open weather map for now

    public class ExternalWeatherAPI
    {
        static HttpClient client;

        static IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

        public static async Task<WeatherAPIData> GetWeatherAsync(string path)
        {
            WeatherAPIData weatherAPIData = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                // weatherAPIData = await response.Content.ReadAsAsync<WeatherAPIData>();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            return weatherAPIData;
        }


        public static void InitClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public static async Task RunAsync()
        {
            if (client is null)
            {
                InitClient();
            }


            try
            {
                var weatherAPIData = await GetWeatherAsync($"/data/2.5/weather?lat=51.5072&lon=0.1276&appid={config["OpenWeatherMapAPI"]}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}