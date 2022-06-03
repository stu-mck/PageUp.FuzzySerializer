using Microsoft.Extensions.DependencyInjection;

namespace PageUp.FuzzySerializer.NetCoreMvc
{
    public static class IMvcBuilderExtensions {

        public static IMvcBuilder AddFuzzySerializer(this IMvcBuilder mvcBuilder) {
            return mvcBuilder.AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new FuzzyObjectContractResolver();
                });
        }

        public static IMvcBuilder AddFuzzySerializer(this IMvcBuilder mvcBuilder, FuzzyObjectContractResolverSettings fuzzySettings) {
            return mvcBuilder.AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new FuzzyObjectContractResolver(fuzzySettings);
                });
        }
        
    }
}
