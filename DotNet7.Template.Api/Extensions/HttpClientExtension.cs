using DotNet7.Template.Api.HttpClients;

namespace DotNet7.Template.Api.Extensions
{
    public class HttpClientExtension
    {
        public static IServiceCollection ConfigureHttpClients(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<SingleSignOnClient>();

            return serviceCollection;
        }
    }
}
