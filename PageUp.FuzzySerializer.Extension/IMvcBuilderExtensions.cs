using Microsoft.Extensions.DependencyInjection;

namespace PageUp.FuzzySerializer.Extension 
{
    public static class IMvcBuilderExtensions {

        public static IMvcBuilder AddFuzzySerializer(this IMvcBuilder mvcBuilder) {
            return mvcBuilder.AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new FuzzyObjectContractResolver();
                });
        }

        public static IMvcBuilder AddFuzzySerializer(this IMvcBuilder mvcBuilder, FuzzyObjectContractResolverSettings fuzzySettings) {
            return mvcBuilder.AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new FuzzyObjectContractResolver(fuzzySettings);
                });
        }
        
    }
}
