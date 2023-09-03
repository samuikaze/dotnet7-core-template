using DotNet7.Template.Api.Models.ServiceModels;

namespace DotNet7.Template.Api.Services
{
    public interface IWeatherService
    {
        public List<WeatherForecastServiceModel> Get();
    }
}
