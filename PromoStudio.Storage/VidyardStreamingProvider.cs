using System.Collections.Generic;
using Newtonsoft.Json;
using PromoStudio.Storage.Vidyard;
using RestSharp;
using PromoStudio.Storage.Properties;

namespace PromoStudio.Storage
{
    public class VidyardStreamingProvider : IStreamingProvider
    {
        private string apiUrl = Settings.Default.VidyardApiUrl;
        private string apiKey = Settings.Default.VidyardApiKey;

        public string StoreFile(string downloadUrl, string videoName)
        {
            var player = CreateVideo(downloadUrl, videoName);
            return player.uuid;
        }

        private Player CreateVideo(string downloadUrl, string videoName)
        {
            var dataObj = new PlayerRequest()
            {
                auth_token = apiKey,
                player = new Player()
                {
                    name = videoName,
                    chapters_attributes = new List<Chapter>()
                    {
                        new Chapter()
                        {
                            position = 0,
                            video_attributes = new Video()
                            {
                                name = videoName,
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
            request.RequestFormat = DataFormat.Json;
            request.AddBody(json);

            IRestResponse<Player> response = client.Execute<Player>(request);
            return response.Data;
        }

        private IRestClient GetClient()
        {
            var client = new RestClient(apiUrl);
            client.Authenticator = new HttpBasicAuthenticator("tim@promostudio.com", "goldmine");
            return client;
        }
    }
}
