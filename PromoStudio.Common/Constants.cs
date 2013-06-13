using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common
{
    public static class Constants
    {
        public static class StoredProcedures
        {
            public const string CustomerInsert_sp = "promostudio.CustomerInsert_sp";
            public const string CustomerSelectById_sp = "promostudio.CustomerSelectById_sp";
            public const string CustomerSelectByLoginCredential_sp = "promostudio.CustomerSelectByLoginCredential_sp";
            public const string CustomerUpdate_sp = "promostudio.CustomerUpdate_sp";
            public const string CustomerDelete_sp = "promostudio.CustomerDelete_sp";

            public const string CustomerLoginCredentialInsertUpdate_sp = "promostudio.CustomerLoginCredentialInsertUpdate_sp";

            public const string CustomerVideoInsert_sp = "promostudio.CustomerVideoInsert_sp";
            public const string CustomerVideoSelectById_sp = "promostudio.CustomerVideoSelectById_sp";
            public const string CustomerVideoSelectByCustomerId_sp = "promostudio.CustomerVideoSelectByCustomerId_sp";
            public const string CustomerVideoSelectForProcessing_sp = "promostudio.CustomerVideoSelectForProcessing_sp";
            public const string CustomerVideoUpdate_sp = "promostudio.CustomerVideoUpdate_sp";
            public const string CustomerVideoDelete_sp = "promostudio.CustomerVideoDelete_sp";

            public const string CustomerVideoItemInsert_sp = "promostudio.CustomerVideoItemInsert_sp";
            public const string CustomerVideoItemSelectById_sp = "promostudio.CustomerVideoItemSelectById_sp";
            public const string CustomerVideoItemSelectByCustomerVideoId_sp = "promostudio.CustomerVideoItemSelectByCustomerVideoId_sp";
            public const string CustomerVideoItemUpdate_sp = "promostudio.CustomerVideoItemUpdate_sp";
            public const string CustomerVideoItemDelete_sp = "promostudio.CustomerVideoItemDelete_sp";

            public const string CustomerVideoVoiceOverInsert_sp = "promostudio.CustomerVideoVoiceOverInsert_sp";
            public const string CustomerVideoVoiceOverSelectById_sp = "promostudio.CustomerVideoVoiceOverSelectById_sp";
            public const string CustomerVideoVoiceOverSelectByCustomerVideoId_sp = "promostudio.CustomerVideoVoiceOverSelectByCustomerVideoId_sp";
            public const string CustomerVideoVoiceOverUpdate_sp = "promostudio.CustomerVideoVoiceOverUpdate_sp";

            public const string CustomerResourceSelectById_sp = "promostudio.CustomerResourceSelectById_sp";
            public const string CustomerResourceSelectActiveByCustomerId_sp = "promostudio.CustomerResourceSelectActiveByCustomerId_sp";
            public const string CustomerResourceSelectByCustomerTemplateScriptId_sp = "promostudio.CustomerResourceSelectByCustomerTemplateScriptId_sp";
            public const string CustomerResourceInsert_sp = "promostudio.CustomerResourceInsert_sp";
            public const string CustomerResourceDelete_sp = "promostudio.CustomerResourceDelete_sp";

            public const string CustomerTemplateScriptSelectById_sp = "promostudio.CustomerTemplateScriptSelectById_sp";
            public const string CustomerTemplateScriptInsert_sp = "promostudio.CustomerTemplateScriptInsert_sp";
            public const string CustomerTemplateScriptUpdate_sp = "promostudio.CustomerTemplateScriptUpdate_sp";

            public const string CustomerTemplateScriptItemSelectById_sp = "promostudio.CustomerTemplateScriptItemSelectById_sp";
            public const string CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp = "promostudio.CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp";
            public const string CustomerTemplateScriptItemInsert_sp = "promostudio.CustomerTemplateScriptItemInsert_sp";
            public const string CustomerTemplateScriptItemUpdate_sp = "promostudio.CustomerTemplateScriptItemUpdate_sp";

            public const string StockAudioSelectAll_sp = "promostudio.StockAudioSelectAll_sp";
            public const string StockAudioSelectByCustomerVideoId_sp = "promostudio.StockAudioSelectByCustomerVideoId_sp";

            public const string StockVideoSelectAll_sp = "promostudio.StockVideoSelectAll_sp";
            public const string StockVideoSelectByCustomerVideoId_sp = "promostudio.StockVideoSelectByCustomerVideoId_sp";

            public const string TemplateScriptSelectAll_sp = "promostudio.TemplateScriptSelectAll_sp";
            public const string TemplateScriptSelectById_sp = "promostudio.TemplateScriptSelectById_sp";

            public const string TemplateScriptItemSelectAll_sp = "promostudio.TemplateScriptItemSelectAll_sp";
            public const string TemplateScriptItemSelectByTemplateScriptId_sp = "promostudio.TemplateScriptItemSelectByTemplateScriptId_sp";
        }
    }
}
