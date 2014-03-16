using System;

namespace PromoStudio.Common.Models
{
    public class CustomerVideoVoiceOver
    {
        public long pk_CustomerVideoVoiceOverId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public int? fk_VoiceActorId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateUploaded { get; set; }
        public string Script { get; set; }
        public string FilePath { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerVideoVoiceOverId,
                fk_CustomerVideoId,
                fk_VoiceActorId,
                DateCreated,
                DateUpdated,
                DateUploaded,
                Script,
                FilePath
            };
        }
    }
}