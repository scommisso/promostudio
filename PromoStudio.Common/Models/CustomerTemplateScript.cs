using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoStudio.Common.Models
{
    public class CustomerTemplateScript
    {
        private List<CustomerTemplateScriptItem> _scriptItems = new List<CustomerTemplateScriptItem>();

        public long pk_CustomerTemplateScriptId { get; set; }
        public long fk_CustomerId { get; set; }
        public long fk_TemplateScriptId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string PreviewFilePath { get; set; }
        public string CompletedFilePath { get; set; }

        public List<CustomerTemplateScriptItem> Items
        {
            get { return _scriptItems; }
            set { _scriptItems = value; }
        }

        public TemplateScript Template { get; set; }

        public string GetSwapItemJson()
        {
            if (Items == null)
            {
                return "[]";
            }
            return string.Format("[{0}]", string.Join(",", Items.Select(i => i.GetSwapItemJson())));
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerTemplateScriptId,
                fk_CustomerId,
                fk_TemplateScriptId,
                DateCreated,
                DateUpdated,
                DateCompleted,
                PreviewFilePath,
                CompletedFilePath
            };
        }
    }
}