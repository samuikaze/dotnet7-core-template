using DotNet7.Template.Api.Services;

namespace DotNet7.Template.Api.ServiceProviders
{
    public class ServiceMapperProvider
    {
        public static IServiceCollection? GetServiceProvider(IServiceCollection? serviceCollection)
        {
            if (serviceCollection != null)
            {
                serviceCollection.AddScoped<IWeatherService, WeatherService>();
            }

            return serviceCollection;
        }
    }
}
