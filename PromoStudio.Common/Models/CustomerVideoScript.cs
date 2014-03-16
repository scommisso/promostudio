namespace PromoStudio.Common.Models
{
    public class CustomerVideoScript
    {
        public long pk_CustomerVideoScriptId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public int fk_AudioScriptTemplateId { get; set; }
        public string ReplacementData { get; set; }

        public AudioScriptTemplate AudioScript { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerVideoScriptId,
                fk_CustomerVideoId,
                fk_AudioScriptTemplateId,
                ReplacementData
            };
        }
    }
}