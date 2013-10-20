namespace PromoStudio.Storage.Vidyard
{
    public class Chapter
    {
        public long? id { get; set; }
        public long? created_at { get; set; }
        public long? updated_at { get; set; }
        public long position { get; set; }
        public long? player_id { get; set; }
        public long? video_id { get; set; }
        public Video video_attributes { get; set; }
    }
}
