using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerVideoScript
    {
        public CustomerVideoScript()
        {
        }

        public CustomerVideoScript(Common.Models.CustomerVideoScript script)
        {
            pk_CustomerVideoScriptId = script.pk_CustomerVideoScriptId;
            fk_CustomerVideoId = script.fk_CustomerVideoId;
            fk_AudioScriptTemplateId = script.fk_AudioScriptTemplateId;
            ReplacementData = script.ReplacementData;
        }

        public long pk_CustomerVideoScriptId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public int fk_AudioScriptTemplateId { get; set; }
        public string ReplacementData { get; set; }

        public Common.Models.CustomerVideoScript ToModel()
        {
            return new Common.Models.CustomerVideoScript
            {
                pk_CustomerVideoScriptId = pk_CustomerVideoScriptId,
                fk_CustomerVideoId = fk_CustomerVideoId,
                fk_AudioScriptTemplateId = fk_AudioScriptTemplateId,
                ReplacementData = ReplacementData
            };
        }
    }
}