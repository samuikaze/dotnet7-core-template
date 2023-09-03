using AutoMapper;
using DotNet7.Template.Api.Models.ServiceModels;

namespace DotNet7.Template.Api.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IMapper _mapper;

        public WeatherService(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<WeatherForecastServiceModel> Get()
        {
            return _mapper.Map<List<WeatherForecastServiceModel>>(
                Enumerable
                    .Range(1, 5)
                    .Select(index => new WeatherForecastServiceModel
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToList());
        }
    }
}
