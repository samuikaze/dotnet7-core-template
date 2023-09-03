using AutoMapper;
using DotNet7.Template.Api.Models.ServiceModels;
using DotNet7.Template.Api.Models.ViewModels;

namespace DotNet7.Template.Api.AutoMapperProfiles
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<WeatherForecastServiceModel, WeatherForecastViewModel>().ReverseMap();
        }
    }
}
