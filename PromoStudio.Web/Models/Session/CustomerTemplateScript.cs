using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerTemplateScript
    {
        public CustomerTemplateScript()
        {
        }

        public CustomerTemplateScript(Common.Models.CustomerTemplateScript templateScript)
        {
            pk_CustomerTemplateScriptId = templateScript.pk_CustomerTemplateScriptId;
            fk_CustomerId = templateScript.fk_CustomerId;
            fk_TemplateScriptId = templateScript.fk_TemplateScriptId;
        }

        public long pk_CustomerTemplateScriptId { get; set; }
        public long fk_CustomerId { get; set; }
        public long fk_TemplateScriptId { get; set; }

        public Common.Models.CustomerTemplateScript ToModel()
        {
            return new Common.Models.CustomerTemplateScript
            {
                pk_CustomerTemplateScriptId = pk_CustomerTemplateScriptId,
                fk_CustomerId = fk_CustomerId,
                fk_TemplateScriptId = fk_TemplateScriptId
            };
        }
    }
}