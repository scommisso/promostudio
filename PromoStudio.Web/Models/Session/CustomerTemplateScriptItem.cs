using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerTemplateScriptItem
    {
        public CustomerTemplateScriptItem()
        {
        }

        public CustomerTemplateScriptItem(Common.Models.CustomerTemplateScriptItem scriptItem)
        {
            pk_CustomerTemplateScriptItemId = scriptItem.pk_CustomerTemplateScriptItemId;
            fk_CustomerTemplateScriptId = scriptItem.fk_CustomerTemplateScriptId;
            fk_TemplateScriptItemId = scriptItem.fk_TemplateScriptItemId;
            fk_CustomerResourceId = scriptItem.fk_CustomerResourceId;
        }

        public long pk_CustomerTemplateScriptItemId { get; set; }
        public long fk_CustomerTemplateScriptId { get; set; }
        public long fk_TemplateScriptItemId { get; set; }
        public long fk_CustomerResourceId { get; set; }

        public Common.Models.CustomerTemplateScriptItem ToModel()
        {
            return new Common.Models.CustomerTemplateScriptItem
            {
                pk_CustomerTemplateScriptItemId = pk_CustomerTemplateScriptItemId,
                fk_CustomerTemplateScriptId = fk_CustomerTemplateScriptId,
                fk_TemplateScriptItemId = fk_TemplateScriptItemId,
                fk_CustomerResourceId = fk_CustomerResourceId
            };
        }
    }
}