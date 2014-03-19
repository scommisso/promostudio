using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerVideo
    {
        public CustomerVideo()
        {
        }

        public CustomerVideo(Common.Models.CustomerVideo video)
        {
            pk_CustomerVideoId = video.pk_CustomerVideoId;
            fk_CustomerId = video.fk_CustomerId;
            fk_StoryboardId = video.fk_StoryboardId;
            Name = video.Name;
            Description = video.Description;
            VimeoVideoId = video.VimeoVideoId;
            VimeoThumbnailUrl = video.VimeoThumbnailUrl;
            VimeoStreamingUrl = video.VimeoStreamingUrl;
            if (video.VoiceOver != null)
            {
                VoiceOver = new CustomerVideoVoiceOver(video.VoiceOver);
            }
            if (video.Script != null)
            {
                Script = new CustomerVideoScript(video.Script);
            }
        }

        public long pk_CustomerVideoId { get; set; }
        public long fk_CustomerId { get; set; }
        public int fk_StoryboardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? VimeoVideoId { get; set; }
        public string VimeoThumbnailUrl { get; set; }
        public string VimeoStreamingUrl { get; set; }

        public CustomerVideoVoiceOver VoiceOver { get; set; }
        public CustomerVideoScript Script { get; set; }

        public Common.Models.CustomerVideo ToModel()
        {
            return new Common.Models.CustomerVideo
            {
                pk_CustomerVideoId = pk_CustomerVideoId,
                fk_CustomerId = fk_CustomerId,
                fk_StoryboardId = fk_StoryboardId,
                Name = Name,
                Description = Description,
                VimeoVideoId = VimeoVideoId,
                VimeoThumbnailUrl = VimeoThumbnailUrl,
                VimeoStreamingUrl = VimeoStreamingUrl,
                Script = Script == null ? null : Script.ToModel(),
                VoiceOver = VoiceOver == null ? null : VoiceOver.ToModel()
            };
        }
    }
}