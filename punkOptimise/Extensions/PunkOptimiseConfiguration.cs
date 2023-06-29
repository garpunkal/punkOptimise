using punkOptimise.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace punkOptimise.Extensions
{
    public static class PunkOptimiseConfiguration
    {
        public static Models.PunkOptimiseConfiguration ConfigureOptimiseConfig(
            this IServiceCollection services, 
            IConfiguration config,
            string configName = "punkOptimiseConfiguration")
        {
            services.Configure<Models.PunkOptimiseConfiguration>(config.GetSection(configName));
            Models.PunkOptimiseConfiguration optimiseConfig = new();
            config.GetSection(configName).Bind(optimiseConfig);
            return optimiseConfig;
        }
    }
}