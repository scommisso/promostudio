using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class AudioScriptTemplate
    {
        public int pk_AudioScriptTemplateId { get; set; }
        public sbyte fk_AudioScriptTemplateStatusId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScriptText { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public AudioScriptTemplateStatus Status
        {
            get { return (AudioScriptTemplateStatus) fk_AudioScriptTemplateStatusId; }
            set { fk_AudioScriptTemplateStatusId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_AudioScriptTemplateId,
                fk_AudioScriptTemplateStatusId,
                fk_OrganizationId,
                fk_VerticalId,
                Name,
                Description,
                ScriptText,
                DateCreated,
                DateUpdated
            };
        }
    }
}