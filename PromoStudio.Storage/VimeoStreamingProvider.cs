using PromoStudio.Common.Enumerations;
using PromoStudio.Storage.Properties;
using System;
using System.IO;
using VimeoDotNet;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace PromoStudio.Storage
{
    public class VimeoStreamingProvider : IStreamingProvider
    {
        private string apiToken = Settings.Default.VimeoApiAccessToken;
        private long presetId = Settings.Default.VimeoApiPresetId;

        private IVimeoClientFactory _vimeoClientFactory;

        public VimeoStreamingProvider(IVimeoClientFactory vimeoClientFactory)
        {
            _vimeoClientFactory = vimeoClientFactory;
        }

        public IUploadRequest StoreFile(string filePath, string videoName, string videoDescription)
        {
            using (var content = new BinaryContent(filePath))
            {
                var client = GetClient();
                var uploadData = client.UploadEntireFile(content);

                if (!uploadData.ClipId.HasValue)
                {
                    throw new VimeoUploadException("Vimeo API did not return a Clip ID", uploadData);
                }

                return uploadData;
            }
        }

        public Video GetVideo(long videoId)
        {
            var client = GetClient();
            return client.GetAccountVideo(videoId);
        }

        private IVimeoClient GetClient()
        {
            return _vimeoClientFactory.GetVimeoClient(apiToken);
        }
    }
}
