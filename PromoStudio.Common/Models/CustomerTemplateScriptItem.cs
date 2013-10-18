namespace PromoStudio.Common.Models
{
    public class CustomerTemplateScriptItem
    {
        public long pk_CustomerTemplateScriptItemId { get; set; }
        public long fk_CustomerTemplateScriptId { get; set; }
        public long fk_TemplateScriptItemId { get; set; }
        public long fk_CustomerResourceId { get; set; }

        public CustomerTemplateScript CustomerScript { get; set; }
        public TemplateScriptItem ScriptItem { get; set; }
        public CustomerResource Resource { get; set; }

        public string GetSwapItemJson()
        {
            return ScriptItem.GetSwapItemJson(Resource.Value);
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerTemplateScriptItemId = pk_CustomerTemplateScriptItemId,
                fk_CustomerTemplateScriptId = fk_CustomerTemplateScriptId,
                fk_TemplateScriptItemId = fk_TemplateScriptItemId,
                fk_CustomerResourceId = fk_CustomerResourceId
            };
        }
    }
}
