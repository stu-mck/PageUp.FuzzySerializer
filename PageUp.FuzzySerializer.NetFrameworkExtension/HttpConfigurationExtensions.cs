using System.Web.Http;

namespace PageUp.FuzzySerializer.NetFrameworkExtension
{
    public static class HttpConfigurationExtensions
    {
        public static void AddFuzzyResponses(this HttpConfiguration configuration, FuzzyObjectContractResolverSettings settings)
        {
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new FuzzyObjectContractResolver(settings);
        }

        public static void AddFuzzyResponses(this HttpConfiguration configuration)
        {
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new FuzzyObjectContractResolver();
        }
    }
}
