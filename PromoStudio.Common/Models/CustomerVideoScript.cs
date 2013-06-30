using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Models
{
    public class CustomerVideoScript
    {
        public long pk_CustomerVideoScriptId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public int fk_AudioTemplateScriptId { get; set; }
        public string ReplacementData { get; set; }

        public AudioTemplateScript AudioScript { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerVideoScriptId = pk_CustomerVideoScriptId,
                fk_CustomerVideoId = fk_CustomerVideoId,
                fk_AudioTemplateScriptId = fk_AudioTemplateScriptId,
                ReplacementData = ReplacementData
            };
        }
    }
}
