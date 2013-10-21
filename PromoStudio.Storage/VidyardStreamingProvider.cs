using Newtonsoft.Json;
using PromoStudio.Storage.Properties;
using PromoStudio.Storage.Vidyard;
using RestSharp;
using System.Collections.Generic;

namespace PromoStudio.Storage
{
    public class VidyardStreamingProvider : IStreamingProvider
    {
        private string apiUrl = Settings.Default.VidyardApiUrl;
        private string apiKey = Settings.Default.VidyardApiKey;
        private string userName = Settings.Default.VidyardApiUserId;
        private string password = Settings.Default.VidyardApiSecret;

        public string StoreFile(string downloadUrl, string videoName, string videoDescription)
        {
            var player = CreateVideo(downloadUrl, videoName, videoDescription);
            return player.uuid;
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

        private IRestClient GetClient()
        {
            var client = new RestClient(apiUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            return client;
        }
    }
}
