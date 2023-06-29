using CSharpTest.Net.Interfaces;
using punkOptimise.Extensions;
using punkOptimise.Interfaces;
using punkOptimise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Extensions;

namespace punkOptimise.Controllers
{
    [PluginController("punkOptimise")]
    public class PunkOptimiseController : UmbracoAuthorizedJsonController
    {
        private readonly IMediaService _mediaService;
        private readonly MediaFileManager _mediaFileManager;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly ILogger<PunkOptimiseController> _logger;
        private readonly MediaUrlGeneratorCollection _mediaUrlGenerators;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly PunkOptimiseProvidersCollection _optimiseCollection;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PunkOptimiseController(
            IMediaService mediaService,
            MediaFileManager mediaFileManager,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, 
            ILogger<PunkOptimiseController> logger, 
            MediaUrlGeneratorCollection mediaUrlGenerators, 
            PunkOptimiseProvidersCollection optimiseCollection, 
            IShortStringHelper shortStringHelper, 
            IHostingEnvironment hostingEnvironment)
        {
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            _mediaFileManager = mediaFileManager ?? throw new ArgumentNullException(nameof(mediaFileManager));
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider ?? throw new ArgumentNullException(nameof(contentTypeBaseServiceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediaUrlGenerators = mediaUrlGenerators ?? throw new ArgumentNullException(nameof(mediaUrlGenerators));
            _shortStringHelper = shortStringHelper ?? throw new ArgumentNullException(nameof(shortStringHelper));
            _optimiseCollection = optimiseCollection ?? throw new ArgumentNullException(nameof(optimiseCollection));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public PunkOptimiseResponse IsValid([FromQuery] int id)
        {
            var media = _mediaService.GetById(id);
            if (media == null)
                return new PunkOptimiseResponse("Media Not Found", Enums.ResultType.Error);

            string file = media.GetUrl("umbracoFile", _mediaUrlGenerators);
            if (media.ContentType.Alias == "Image")
            {
                string extension = _mediaFileManager.FileSystem.GetExtension(file)?.Substring(1);
                foreach (IPunkOptimiserProvider provider in _optimiseCollection)                
                    if (provider.IsValid(new string[] { extension }))                    
                        return new PunkOptimiseResponse(provider.Name, resultType: Enums.ResultType.Success);
            }

            return new PunkOptimiseResponse("Media Not Valid", Enums.ResultType.Error);
        }

        public async Task<PunkOptimiseResponse> Save([FromBody] SaveModel model)
        {
            var token = new CancellationTokenSource().Token;

            var media = _mediaService.GetById(model.Id);
            if (media == null) return new PunkOptimiseResponse("Media Not Found", Enums.ResultType.Error);

            string file = media.GetUrl("umbracoFile", _mediaUrlGenerators);
                     
            var path = _hostingEnvironment.ToAbsolute(file);
            string extension = _mediaFileManager.FileSystem.GetExtension(file)?.Substring(1);

            if (_mediaFileManager.FileSystem.FileExists(path))
            {
                foreach (IPunkOptimiserProvider provider in _optimiseCollection)
                {
                    if (!provider.IsValid(new string[] { extension }))
                        continue;

                    var bytes = LoadData(path);
                    if (bytes?.Length > 0)
                    {
                        var data = await provider.Process(bytes, token);
                        if (data != null)                        
                            return SaveMedia(media, file, true, data);
                        
                    }
                }
            }

            return new PunkOptimiseResponse("Error Logged", Enums.ResultType.Error);
        }

        private byte[] LoadData(string fullPath)
        {            
            byte[] data = null;
            try
            {
                using (Stream inStream = _mediaFileManager.FileSystem.OpenFile(fullPath))
                {
                    inStream.Seek(0, SeekOrigin.Begin);
                    data = inStream.ReadAsBytes();
                }
            }
            catch { }
            return data;
        }

        private PunkOptimiseResponse SaveMedia(IMedia media, string path, bool updated, byte[] data)
        {
            if (data != null || updated)
            {
                using (var outStream = new MemoryStream(data))
                {
                    outStream.Position = 0;
                    try
                    {
                        media.SetValue(
                            _mediaFileManager,
                            _mediaUrlGenerators,
                            _shortStringHelper,
                            _contentTypeBaseServiceProvider,
                            "umbracoFile",
                            Path.GetFileName(path),
                            outStream);

                        _mediaService.Save(media);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Save", ex);
                        throw;
                    }
                }

                return new PunkOptimiseResponse("Media optimised without any errors", Enums.ResultType.Success);
            }
            else
                return new PunkOptimiseResponse("Media optimising was unsuccessful", Enums.ResultType.Error);
        }
    }
}

