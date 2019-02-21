using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core.Models;

namespace Weather.OpenWeatherMap
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private readonly string _appId;
        public Client(HttpClient httpClient, string appId)
        {
            this._httpClient = httpClient;
            this._appId = appId;
        }

        public async Task<Weather.Core.Models.WeatherResponse> Current(Coord coords)
        {
            var result = await _httpClient.GetStringAsync($"http://api.openweathermap.org/data/2.5/weather?lat={coords.Lat}&lon={coords.Lon}&appid={this._appId}&units=imperial");
            Weather.Core.Models.WeatherResponse weatherResponse = JsonConvert.DeserializeObject<Weather.Core.Models.WeatherResponse>(result);
            return weatherResponse;
        }

        public async Task<Weather.Core.Models.WeatherResponse[]> Forecast(Coord coords)
        {
            var results = await _httpClient.GetStringAsync($"http://api.openweathermap.org/data/2.5/forecast?lat={coords.Lat}&lon={coords.Lon}&appid={this._appId}&units=imperial");
            JObject jObject = JObject.Parse(results);
            var forecastList = jObject.SelectToken("list").ToString(Formatting.None);
            Weather.Core.Models.WeatherResponse[] weatherResponse = JsonConvert.DeserializeObject<Weather.Core.Models.WeatherResponse[]>(forecastList);
            return weatherResponse;
        }
    }
}
