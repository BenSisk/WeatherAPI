# WeatherAPI

## API Usage

### Exposed Endpoints
- /weather
  - lat (optional - defaults to London)
  - long (optional, defaults to London)
  - TempUnit (optional - defaults to Celsius, possible values: "C", "F", "K")
  - API (optional - defaults to OpenWeatherMap, possible values: "OpenWeatherMap", "WeatherBit")

- /weather/date-range
    - StartDate (required, format yyyy-mm-dd, cannot be in the future)
    - EndDate (required, format yyyy-mm-dd, must not be before StartDate)
    - lat (optional - defaults to London)
    - long (optional - defaults to London)
    - TempUnit (optional - defaults to Celsius, possible values: "C", "F", "K")


## Unplanned possible improvements:

- Geocoding location / city names with a city variable rather than long/lat
- Data Visualisation
- Caching


## Weather Data Sources

Weather data provided by [WeatherBit](https://www.weatherbit.io/)

Weather data provided by [OpenWeather](https://openweathermap.org/)

Weather data provided by [Open-Meteo](https://open-meteo.com/)

![OpenWeather-Master-Logo RGB](https://github.com/BenSisk/WeatherAPI/assets/43730029/02401a9a-d255-46c7-a16e-6508caca7fd9)
