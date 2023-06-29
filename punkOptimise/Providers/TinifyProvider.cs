using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using punkOptimise.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinifyAPI;

namespace punkOptimise.Providers
{
    public class TinifyProvider : PunkOptimiserProviderBase
    {
        private readonly PunkOptimiseConfiguration _optimiseConfiguration;
        private readonly ILogger<TinifyProvider> _logger;
        public TinifyProvider(IOptions<PunkOptimiseConfiguration> optimiseConfiguration, ILogger<TinifyProvider> logger)
            : base(optimiseConfiguration, logger)
        {
            _optimiseConfiguration = optimiseConfiguration.Value ?? throw new ArgumentNullException(nameof(optimiseConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override string Name => nameof(TinifyProvider);
        public override string Description => "TinyPNG: api.tinify.com";
        public override bool IsValid(string[] extensions)
        {
            if (!(extensions?.Any() ?? false))
                return false;

            return extensions.Any(x => x switch
            {
                "png" => true,
                "webp" => true,
                _ => false,
            });
        }

        public override async Task<byte[]> Process(byte[] data, CancellationToken token)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_optimiseConfiguration.TinyPng.ApiKey))
                {
                    Tinify.Key = _optimiseConfiguration.TinyPng.ApiKey;
                    return await Tinify.FromBuffer(data).ToBuffer();
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}