using System.Collections.Generic;

namespace PromoStudio.Storage.Vidyard
{
    public class Video
    {
        public string name { get; set; }
        public string upload_url { get; set; }
        public string description { get; set; }
        public string youtube_category { get; set; }
        public bool? sync { get; set; }
        public long? audio_gain { get; set; }
        public string webhook_url { get; set; }
        public long? id { get; set; }
        public string status { get; set; }
        public string error_message { get; set; }
        public long? created_at { get; set; }
        public long? updated_at { get; set; }
        public List<Tag> tags_attributes { get; set; }
    }
}
