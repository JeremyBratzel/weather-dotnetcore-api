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
    public class ForecastController : ControllerBase
    {
        private readonly Client _client;
        public ForecastController(Client client)
        {
            this._client = client;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get(string lat, string lon)
        {
            if (Double.TryParse(lat, out double latitude) && Double.TryParse(lon, out double longitude))
            {
                try
                {
                    var weatherResponses = await _client.Forecast(new Coord { Lat = latitude, Lon = longitude });
                    return Ok(weatherResponses.Select(weatherResponse => new WeatherResult { Date = DateTimeOffset.FromUnixTimeSeconds(weatherResponse.Dt).DateTime, Temp = weatherResponse.Main.Temp, Icon = weatherResponse.Weather[0].Icon }));
                }
                catch(Exception e)
                {
                    return StatusCode(500, "Error retrieving weather information");
                }
            }
            return BadRequest("Coordinates were missing or incorrect");
        }

    }
}