using Newtonsoft.Json;
using PromoStudio.Common.Enumerations;
using PromoStudio.Storage.Properties;
using PromoStudio.Storage.Vidyard;
using RestSharp;
using System;
using System.Collections.Generic;

namespace PromoStudio.Storage
{
    public class VidyardStreamingProvider : IStreamingProvider
    {
        private string apiUrl = Settings.Default.VidyardApiUrl;
        private string apiKey = Settings.Default.VidyardApiKey;
        private string userName = Settings.Default.VidyardApiUserId;
        private string password = Settings.Default.VidyardApiSecret;

        public Player StoreFile(string downloadUrl, string videoName, string videoDescription)
        {
            var player = CreateVideo(downloadUrl, videoName, videoDescription);
            return player;
        }

        private Player CreateVideo(string downloadUrl, string videoName, string videoDescription)
        {
            var dataObj = new PlayerRequest()
            {
                auth_token = apiKey,
                player = new Player()
                {
                    name = videoName,
                    embed_button = false,
                    hd_button = true,
                    play_button = true,
                    width = 640,
                    height = 360,
                    color = "aaaaaa",
                    default_hd = true,
                    autoplay = false,
                    mute_onload = false,
                    playlist_always_open = false,
                    chapters_attributes = new List<Chapter>()
                    {
                        new Chapter()
                        {
                            position = 0,
                            video_attributes = new Video()
                            {
                                name = videoName,
                                description = videoDescription,
                                upload_url = downloadUrl
                            }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(dataObj, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var client = GetClient();
            var request = new RestRequest("players.json", Method.POST);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse<Player> response = client.Execute<Player>(request);
            return response.Data;
        }

        public CloudStorageStatus GetFileStatus(long videoId)
        {
            var client = GetClient();
            var request = new RestRequest(string.Format("videos/{0}?auth_token={1}", videoId, apiKey), Method.GET);
            IRestResponse<Video> response = client.Execute<Video>(request);
            
            if (response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.error_message))
                {
                    return CloudStorageStatus.Errored;
                }
                if (string.Compare(response.Data.status, "ready", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return CloudStorageStatus.Completed;
                }
            }
            return CloudStorageStatus.InProgress;
        }

        private IRestClient GetClient()
        {
            var client = new RestClient(apiUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            return client;
        }
    }
}
