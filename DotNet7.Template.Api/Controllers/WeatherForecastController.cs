using AutoMapper;
using DotNet7.Template.Api.Models.ViewModels;
using DotNet7.Template.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNet7.Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("氣象預報")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMapper _mapper;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IMapper mapper,
            IWeatherService weatherService)
        {
            _logger = logger;
            _mapper = mapper;
            _weatherService = weatherService;
        }

        /// <summary>
        /// 取得天氣預報
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWeatherForecast")]
        public List<WeatherForecastViewModel> Get()
        {
            return _mapper.Map<List<WeatherForecastViewModel>>(
                _weatherService.Get());
        }
    }
}
