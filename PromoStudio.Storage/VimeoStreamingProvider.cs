using PromoStudio.Common.Enumerations;
using PromoStudio.Storage.Properties;
using PromoStudio.Storage.Vidyard;
using System;
using System.IO;
using Vimeo.API;

namespace PromoStudio.Storage
{
    public class VimeoStreamingProvider : IStreamingProvider
    {
        private string clientId = Settings.Default.VimeoApiClientId;
        private string clientSecret = Settings.Default.VimeoApiClientSecret;
        private string apiToken = Settings.Default.VimeoApiAccessToken;
        private string apiSecret = Settings.Default.VimeoApiAccessTokenSecret;
        private long presetId = Settings.Default.VimeoApiPresetId;

        public Player StoreFile(string downloadUrl, string videoName, string videoDescription)
        {
            var client = GetClient();

            var quota = client.vimeo_videos_upload_getQuota();

            var fileName = Path.GetFileName(downloadUrl);
            var fileStream = System.IO.File.OpenRead(downloadUrl);
            var contentType = System.Web.MimeMapping.GetMimeMapping(fileName);
            if (fileStream.Length > quota.free)
            {
                throw new ApplicationException("Vimeo account has run out of storage space");
            }

            var ticket = client.vimeo_videos_upload_getTicket(uploadMethod: "streaming");
            if (!client.vimeo_videos_upload(ticket, fileStream, contentType))
            {
                throw new ApplicationException("Error uploading file");
            }

            if (!client.vimeo_videos_upload_isstreamcomplete(ticket, fileStream, contentType))
            {
                throw new ApplicationException("Upload incomplete");
            }

            string videoId = client.vimeo_videos_upload_complete(fileName, ticket);

            client.vimeo_videos_setTitle(videoId, videoName);
            client.vimeo_videos_setDescription(videoId, videoDescription);
            client.vimeo_videos_setDownloadPrivacy(videoId, false);
            client.vimeo_videos_setLicense(videoId, VimeoClient.VideoLicenses.None);
            client.vimeo_videos_embed_setPreset(videoId, presetId.ToString());

            var video = client.vimeo_videos_getInfo(videoId);
            // TODO: return video

            return new Player()
            {
                id = long.Parse(video.id)
            };
        }

        public CloudStorageStatus GetFileStatus(long videoId)
        {
            var client = GetClient();

            var video = client.vimeo_videos_getInfo(videoId.ToString());

            if (video.is_transcoding == "1")
            {
                return CloudStorageStatus.Completed;
            }
            //var client = GetClient();
            //var request = new RestRequest(string.Format("videos/{0}?auth_token={1}", videoId, apiKey), Method.GET);
            //IRestResponse<Video> response = client.Execute<Video>(request);
            
            //if (response.Data != null)
            //{
            //    if (!string.IsNullOrEmpty(response.Data.error_message))
            //    {
            //        return CloudStorageStatus.Errored;
            //    }
            //    if (string.Compare(response.Data.status, "ready", StringComparison.OrdinalIgnoreCase) == 0)
            //    {
            //        return CloudStorageStatus.Completed;
            //    }
            //}
            return CloudStorageStatus.InProgress;
        }

        private VimeoClient GetClient()
        {
            var client = new VimeoClient(clientId, clientSecret);
            if (!client.Login(apiToken, apiSecret))
            {
                throw new UnauthorizedAccessException("Invalid token for Vimeo API");
            }
            return client;
        }
    }
}
