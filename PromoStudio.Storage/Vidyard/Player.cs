using System.Collections.Generic;

namespace PromoStudio.Storage.Vidyard
{
    public class Player
    {
        public string access_code { get; set; }
        public bool? autoplay { get; set; }
        public bool? sharing_page_comments { get; set; }
        public bool? sharing_page { get; set; }
        public bool? default_hd { get; set; }
        public bool? splash_screen_fade { get; set; }
        public bool? mute_onload { get; set; }
        public string name { get; set; }
        public long? height { get; set; }
        public long? width { get; set; }
        public bool? playlist_always_open { get; set; }
        public long? release_date { get; set; }
        public string redirect_url { get; set; }
        public bool? redirect_whole_page { get; set; }
        public string color { get; set; }
        public bool? hd_button { get; set; }
        public bool? play_button { get; set; }
        public bool? viral_sharing { get; set; }
        public string whitelisted_embed_domain { get; set; }
        public bool? embed_button { get; set; }
        public string final_call_to_action { get; set; }
        public long? id { get; set; }
        public string uuid { get; set; }
        public string status { get; set; }
        public long? created_at { get; set; }
        public long? updated_at { get; set; }
        public List<Chapter> chapters_attributes { get; set; }
    }
}
