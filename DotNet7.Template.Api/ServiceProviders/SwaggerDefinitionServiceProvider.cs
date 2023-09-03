using System.Reflection;

namespace DotNet7.Template.Api.ServiceProviders
{
    public class SwaggerDefinitionServiceProvider
    {
        public static IServiceCollection ConfigureSwagger(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(config =>
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
                config.IncludeXmlComments(filePath);
                config.EnableAnnotations();
            });

            return serviceCollection;
        }
    }
}
