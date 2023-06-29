using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using punkOptimise.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace punkOptimise.Providers
{
    public class ImageSharpProvider : PunkOptimiserProviderBase
    {
        private readonly PunkOptimiseConfiguration _optimiseConfiguration;
        private readonly ILogger<ImageSharpProvider> _logger;
        public ImageSharpProvider(IOptions<PunkOptimiseConfiguration> optimiseConfiguration, ILogger<ImageSharpProvider> logger)
            : base(optimiseConfiguration, logger)
        {
            _optimiseConfiguration = optimiseConfiguration.Value ?? throw new ArgumentNullException(nameof(optimiseConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override string Name => nameof(ImageSharpProvider);
        public override string Description => "Supports JPG and JPEG files.";
        public override bool IsValid(string[] extensions)
        {
            if (!(extensions?.Any() ?? false))
                return false;

            return extensions.Any(x => x switch
            {
                "jpeg" or "jpg" => true,
                _ => false,
            });
        }

        public override async Task<byte[]> Process(byte[] data, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (data == null) throw new ArgumentNullException(nameof(data));

                    using MemoryStream ms = new();

                    IImageEncoder imageEncoderForJpeg = new JpegEncoder()
                    {
                        Quality = _optimiseConfiguration.Quality,
                    };

                    using (var image = Image.Load(data))
                    {
                        image.Metadata.ExifProfile = null;
                        image.Save(ms, imageEncoderForJpeg);
                    }

                    return ms.ToArray();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return null;
                }
            });
        }
    }
}