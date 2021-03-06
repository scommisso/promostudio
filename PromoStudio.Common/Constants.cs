﻿using System;
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
            public const string AudioScriptTemplateDelete_sp = "promostudio.AudioScriptTemplateDelete_sp";
            public const string AudioScriptTemplateInsert_sp = "promostudio.AudioScriptTemplateInsert_sp";
            public const string AudioScriptTemplateSelectAll_sp = "promostudio.AudioScriptTemplateSelectAll_sp";
            public const string AudioScriptTemplateSelectByCustomerVideoId_sp = "promostudio.AudioScriptTemplateSelectByCustomerVideoId_sp";
            public const string AudioScriptTemplateSelectById_sp = "promostudio.AudioScriptTemplateSelectById_sp";
            public const string AudioScriptTemplateUpdate_sp = "promostudio.AudioScriptTemplateUpdate_sp";

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
            public const string CustomerVideoSelectForCloudStatusCheck_sp = "promostudio.CustomerVideoSelectForCloudStatusCheck_sp";
            public const string CustomerVideoUpdate_sp = "promostudio.CustomerVideoUpdate_sp";
            public const string CustomerVideoDelete_sp = "promostudio.CustomerVideoDelete_sp";

            public const string CustomerVideoItemInsert_sp = "promostudio.CustomerVideoItemInsert_sp";
            public const string CustomerVideoItemSelectById_sp = "promostudio.CustomerVideoItemSelectById_sp";
            public const string CustomerVideoItemSelectByCustomerVideoId_sp = "promostudio.CustomerVideoItemSelectByCustomerVideoId_sp";
            public const string CustomerVideoItemUpdate_sp = "promostudio.CustomerVideoItemUpdate_sp";
            public const string CustomerVideoItemDelete_sp = "promostudio.CustomerVideoItemDelete_sp";

            public const string CustomerVideoScriptInsert_sp = "promostudio.CustomerVideoScriptInsert_sp";
            public const string CustomerVideoScriptSelectByCustomerVideoId_sp = "promostudio.CustomerVideoScriptSelectByCustomerVideoId_sp";
            public const string CustomerVideoScriptSelectById_sp = "promostudio.CustomerVideoScriptSelectById_sp";

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

            public const string OrganizationSelectById_sp = "promostudio.OrganizationSelectById_sp";

            public const string StockAudioSelectAll_sp = "promostudio.StockAudioSelectAll_sp";
            public const string StockAudioSelectByCustomerVideoId_sp = "promostudio.StockAudioSelectByCustomerVideoId_sp";
            public const string StockAudioSelectByOrganizationIdAndVerticalId_sp = "promostudio.StockAudioSelectByOrganizationIdAndVerticalId_sp";

            public const string StockVideoSelectAll_sp = "promostudio.StockVideoSelectAll_sp";
            public const string StockVideoSelectByCustomerVideoId_sp = "promostudio.StockVideoSelectByCustomerVideoId_sp";
            public const string StockVideoSelectByOrganizationIdAndVerticalId_sp = "promostudio.StockVideoSelectByOrganizationIdAndVerticalId_sp";
            public const string StockVideoSelectByStoryboardId_sp = "promostudio.StockVideoSelectByStoryboardId_sp";

            public const string StoryboardInsert_sp = "promostudio.StoryboardInsert_sp";
            public const string StoryboardUpdate_sp = "promostudio.StoryboardUpdate_sp";
            public const string StoryboardDelete_sp = "promostudio.StoryboardDelete_sp";
            public const string StoryboardSelectAll_sp = "promostudio.StoryboardSelectAll_sp";
            public const string StoryboardSelectById_sp = "promostudio.StoryboardSelectById_sp";
            public const string StoryboardSelectByCustomerVideoId_sp = "promostudio.StoryboardSelectByCustomerVideoId_sp";
            public const string StoryboardSelectByOrganizationIdAndVerticalId_sp = "promostudio.StoryboardSelectByOrganizationIdAndVerticalId_sp";

            public const string StoryboardItemDelete_sp = "promostudio.StoryboardItemDelete_sp";
            public const string StoryboardItemInsert_sp = "promostudio.StoryboardItemInsert_sp";
            public const string StoryboardItemSelectByCustomerVideoId_sp = "promostudio.StoryboardItemSelectByCustomerVideoId_sp";
            public const string StoryboardItemSelectById_sp = "promostudio.StoryboardItemSelectById_sp";
            public const string StoryboardItemSelectByStoryboardId_sp = "promostudio.StoryboardItemSelectByStoryboardId_sp";

            public const string TemplateScriptSelectAll_sp = "promostudio.TemplateScriptSelectAll_sp";
            public const string TemplateScriptSelectById_sp = "promostudio.TemplateScriptSelectById_sp";
            public const string TemplateScriptSelectByOrganizationIdAndVerticalId_sp = "promostudio.TemplateScriptSelectByOrganizationIdAndVerticalId_sp";
            public const string TemplateScriptSelectByStoryboardId_sp = "promostudio.TemplateScriptSelectByStoryboardId_sp";

            public const string TemplateScriptItemSelectAll_sp = "promostudio.TemplateScriptItemSelectAll_sp";
            public const string TemplateScriptItemSelectByTemplateScriptId_sp = "promostudio.TemplateScriptItemSelectByTemplateScriptId_sp";
            public const string TemplateScriptItemSelectByStoryboardId_sp = "promostudio.TemplateScriptItemSelectByStoryboardId_sp";
            
            public const string VoiceActorSelectAll_sp = "promostudio.VoiceActorSelectAll_sp";
        }
    }
}
