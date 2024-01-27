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

### Example Requests

### Example 1

`GET /weather`

longitude = -0.1149 (Default)  
latitude = 51.5142 (Default)  
Temperature unit: Celsius (Default)  
Query API: OpenWeatherMap (Default)

    http://localhost:5074/Weather

Produced Response:

```json
{
  "date": "2024-01-27T19:00:28.3551942+00:00",
  "longitude": -0.1149,
  "latitude": 51.5142,
  "tempUnit": "c",
  "temp": 5.16,
  "humidity": 85,
  "windSpeed": 2.06,
  "windDirection": 130,
  "pressure": 1031,
  "cloudCover": 100
}
```

### Example 2

`GET /weather`

longitude = -118.2426  
latitude = 34.0549  
Temperature unit: Celsius (Default)  
Query API: OpenWeatherMap (Default)

    http://localhost:5074/Weather?Long=-118.2426&Lat=34.0549

Produced Response:

```json
{
  "date": "2024-01-27T19:00:50.9065155+00:00",
  "longitude": -118.2426,
  "latitude": 34.0549,
  "tempUnit": "c",
  "temp": 21.86,
  "humidity": 34,
  "windSpeed": 1.54,
  "windDirection": 50,
  "pressure": 1030,
  "cloudCover": 0
}
```

### Example 3

`GET /weather`

longitude = -118.2426  
latitude = 34.0549  
Temperature unit: Kelvin  
Query API: WeatherBit

    http://localhost:5074/Weather?Long=-118.2426&Lat=34.0549&TempUnit=K&API=WeatherBit

Produced Response:

```json
{
  "date": "2024-01-27T19:01:19.1442357+00:00",
  "longitude": -118.2426,
  "latitude": 34.0549,
  "tempUnit": "K",
  "temp": 294.45,
  "humidity": 96.34,
  "windSpeed": 0.42993164,
  "windDirection": 214,
  "pressure": 1011.5,
  "cloudCover": 0
}
```

### Example 4

`GET /weather/date-range`

longitude = -0.1149 (Default)  
latitude = 51.5142 (Default)  
Temperature unit: Celsius (Default)  
Start Date: 1st January 2023  
End Date: 1st January 2024

    http://localhost:5074/weather/date-range&startdate=2023-1&enddate=2024-1

Produced Response:

```json
{
  "startDate": "2023-01-01T00:00:00",
  "endDate": "2024-01-01T00:00:00",
  "longitude": -0.11489,
  "latitude": 51.51418,
  "tempUnit": "C",
  "minTemp": -4.8,
  "maxTemp": 31.3,
  "avgTemp": 11.56
}
```

### Example 5

`GET /weather/date-range`

longitude = -118.2426  
latitude = 34.0549  
Temperature unit: Celsius (Default)  
Start Date: 1st April 2023  
End Date: 24th January 2024

    http://localhost:5074/weather/date-range&startdate=2023-4-1&enddate=2024-1-24?Long=-118.2426&Lat=34.0549

Produced Response:

```json
{
  "startDate": "2023-04-01T00:00:00",
  "endDate": "2024-01-24T00:00:00",
  "longitude": -118.2426,
  "latitude": 34.0549,
  "tempUnit": "C",
  "minTemp": 1.9,
  "maxTemp": 36,
  "avgTemp": 17.88
}
```

### Example 6

`GET /weather/date-range`

longitude = -118.2426  
latitude = 34.0549  
Temperature unit: Kelvin  
Start Date: 8th March 2000  
End Date: 16th December 2020

    http://localhost:5074/weather/date-range&startdate=2000-03-08&enddate=2020-12-16?Long=-118.2426&Lat=34.0549&TempUnit=K

Produced Response:

```json
{
  "startDate": "2000-03-08T00:00:00",
  "endDate": "2020-12-16T00:00:00",
  "longitude": -118.2426,
  "latitude": 34.0549,
  "tempUnit": "K",
  "minTemp": 271.15,
  "maxTemp": 317.95,
  "avgTemp": 291.09
}
```

## Unplanned possible improvements:

- Geocoding location / city names with a city variable rather than long/lat
- Data Visualisation
- Caching


## Weather Data Sources

Weather data provided by [WeatherBit](https://www.weatherbit.io/)

Weather data provided by [OpenWeather](https://openweathermap.org/)

Weather data provided by [Open-Meteo](https://open-meteo.com/)

![OpenWeather-Master-Logo RGB](https://github.com/BenSisk/WeatherAPI/assets/43730029/02401a9a-d255-46c7-a16e-6508caca7fd9)
