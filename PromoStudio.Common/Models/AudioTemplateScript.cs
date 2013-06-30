using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromoStudio.Common.Models
{
    public class AudioTemplateScript
    {
        public int pk_AudioScriptTemplateId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScriptText { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_AudioScriptTemplateId = pk_AudioScriptTemplateId,
                fk_OrganizationId = fk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                Description = Description,
                ScriptText = ScriptText
            };
        }
    }
}
