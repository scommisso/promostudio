using System.Collections.Generic;

namespace PromoStudio.Storage.Vidyard
{
    public class Tag
    {
        public long? id { get; set; }
        public string name { get; set; }
        public List<long> video_ids { get; set; }
        public long? created_at { get; set; }
    }
}
