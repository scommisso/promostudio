using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerVideoVoiceOver
    {
        public long pk_CustomerVideoVoiceOverId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public int? fk_VoiceActorId { get; set; }
        public string Script { get; set; }
        public string FilePath { get; set; }

        public CustomerVideoVoiceOver()
        {
        }

        public CustomerVideoVoiceOver(Common.Models.CustomerVideoVoiceOver voiceOver)
        {
            pk_CustomerVideoVoiceOverId = voiceOver.pk_CustomerVideoVoiceOverId;
            fk_CustomerVideoId = voiceOver.fk_CustomerVideoId;
            fk_VoiceActorId = voiceOver.fk_VoiceActorId;
            Script = voiceOver.Script;
            FilePath = voiceOver.FilePath;
        }

        public Common.Models.CustomerVideoVoiceOver ToModel()
        {
            return new Common.Models.CustomerVideoVoiceOver()
            {
                pk_CustomerVideoVoiceOverId = pk_CustomerVideoVoiceOverId,
                fk_CustomerVideoId = fk_CustomerVideoId,
                fk_VoiceActorId = fk_VoiceActorId,
                Script = Script,
                FilePath = FilePath
            };
        }
    }
}