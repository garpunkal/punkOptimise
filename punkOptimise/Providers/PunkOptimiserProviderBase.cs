using punkOptimise.Interfaces;
using punkOptimise.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace punkOptimise.Providers
{
    public abstract class PunkOptimiserProviderBase : IPunkOptimiserProvider
    {
        private readonly PunkOptimiseConfiguration _optimiseConfiguration;
        private readonly ILogger<IPunkOptimiserProvider> _logger;
        protected PunkOptimiserProviderBase(
            IOptions<PunkOptimiseConfiguration> optimiseConfiguration,
            ILogger<IPunkOptimiserProvider> logger)
        {
            _optimiseConfiguration = optimiseConfiguration.Value ?? throw new ArgumentNullException(nameof(optimiseConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual string Name => nameof(PunkOptimiserProviderBase);
        public virtual string Description => string.Empty;
        public virtual bool IsValid(string[] extensions) => true;                   
        public virtual async Task<byte[]> Process(byte[] data, CancellationToken token) => await Task.Run(() => data);
        
    }
}