using AutoMapper;
using DotNet7.Template.Api.Filters;
using DotNet7.Template.Api.Models.ServiceModels;
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

        /// <summary>
        /// 取得使用者資訊 (範例用，由於未宣告 SSO 網址，因此會得到 500 回應)
        /// </summary>
        /// <returns></returns>
        [HttpGet("User")]
        [TypeFilter(typeof(VerifyAccessTokenAuthorizeAttribute))]
        public string GetUser()
        {
            UserServiceModel user = (UserServiceModel)HttpContext.Items["User"]!;

            return $"I am {user.Username}";
        }
    }
}
