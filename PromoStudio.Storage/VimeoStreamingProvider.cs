using PromoStudio.Storage.Properties;
using VimeoDotNet;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace PromoStudio.Storage
{
    public class VimeoStreamingProvider : IStreamingProvider
    {
        private readonly IVimeoClientFactory _vimeoClientFactory;
        private readonly string apiToken = Settings.Default.VimeoApiAccessToken;
        private long presetId = Settings.Default.VimeoApiPresetId;

        public VimeoStreamingProvider(IVimeoClientFactory vimeoClientFactory)
        {
            _vimeoClientFactory = vimeoClientFactory;
        }

        public IUploadRequest StoreFile(string filePath, string videoName, string videoDescription)
        {
            using (var content = new BinaryContent(filePath))
            {
                IVimeoClient client = GetClient();
                IUploadRequest uploadData = client.UploadEntireFile(content);

                if (!uploadData.ClipId.HasValue)
                {
                    throw new VimeoUploadException("Vimeo API did not return a Clip ID", uploadData);
                }

                return uploadData;
            }
        }

        public Video GetVideo(long videoId)
        {
            IVimeoClient client = GetClient();
            return client.GetAccountVideo(videoId);
        }

        private IVimeoClient GetClient()
        {
            return _vimeoClientFactory.GetVimeoClient(apiToken);
        }
    }
}