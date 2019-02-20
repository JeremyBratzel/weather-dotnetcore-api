using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weather.Core.Models;
using Weather.OpenWeatherMap;

namespace WeatherApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly Client _client;
        public WeatherController(Client client)
        {
            this._client = client;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<WeatherResult>> Get(string lat, string lon)
        {
            if (Double.TryParse(lat, out double latitude) && Double.TryParse(lon, out double longitude))
            {
                try
                {
                    var weatherResponse = await _client.Current(new Coord { lat = latitude, lon = longitude });
                    return new WeatherResult { date = DateTimeOffset.FromUnixTimeSeconds(weatherResponse.dt).DateTime, temp = weatherResponse.main.temp, icon = weatherResponse.weather[0].icon };
                }
                catch
                {
                    return StatusCode(500, "Error retrieving weather information");
                }
            }
            return BadRequest("Coordinates were missing or incorrect");
            //return await _client.Current(new Coord { lat=42.3305216, lon=-83.0464 });
        }

    }
}