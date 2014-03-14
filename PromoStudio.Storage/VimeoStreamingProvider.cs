using PromoStudio.Common.Enumerations;
using PromoStudio.Storage.Properties;
using PromoStudio.Storage.Vidyard;
using System;
using System.IO;
using VimeoDotNet;
using VimeoDotNet.Net;

namespace PromoStudio.Storage
{
    public class VimeoStreamingProvider : IStreamingProvider
    {
        private string apiToken = Settings.Default.VimeoApiAccessToken;
        private long presetId = Settings.Default.VimeoApiPresetId;

        public Player StoreFile(string downloadUrl, string videoName, string videoDescription)
        {
            var client = GetClient();
            var uploadData = client.UploadEntireFile(new BinaryContent(downloadUrl));

            // TODO: update video name, description, etc

            return new Player()
            {
                id = uploadData.ClipId
            };
        }

        public CloudStorageStatus GetFileStatus(long videoId)
        {
            var client = GetClient();

            // TODO: Get transcoding status

            return CloudStorageStatus.InProgress;
        }

        private VimeoClient GetClient()
        {
            var client = new VimeoClient(apiToken);
            return client;
        }
    }
}
