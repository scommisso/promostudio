using System;
namespace PromoStudio.Data
{
    public interface IDataService
    {
        System.Threading.Tasks.Task<PromoStudio.Common.Models.AudioTemplateScript> AudioTemplateScript_InsertAsync(PromoStudio.Common.Models.AudioTemplateScript audioScript);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.AudioTemplateScript>> AudioTemplateScript_SelectAllAsync();
        System.Threading.Tasks.Task<PromoStudio.Common.Models.AudioTemplateScript> AudioTemplateScript_SelectAsync(long audioTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.AudioTemplateScript> AudioTemplateScript_SelectByCustomerVideoIdAsync(long customerVideoId);
        void AudioTemplateScript_Update(PromoStudio.Common.Models.AudioTemplateScript audioScript);
        void Customer_Delete(long customerId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.Customer> Customer_InsertAsync(PromoStudio.Common.Models.Customer customer);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.Customer> Customer_SelectAsync(long customerId);
        void Customer_Update(PromoStudio.Common.Models.Customer customer);
        void CustomerLoginCredential_InsertUpdate(PromoStudio.Common.Models.CustomerLoginCredential customerLoginCredential);
        void CustomerResource_Delete(long customerResourceId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerResource> CustomerResource_InsertAsync(PromoStudio.Common.Models.CustomerResource customerResource);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerResource> CustomerResource_Select(long customerResourceId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerResource>> CustomerResource_SelectActiveByCustomerIdAsync(long customerId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerResource>> CustomerResource_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_InsertAsync(PromoStudio.Common.Models.CustomerTemplateScript customerTemplateScript);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(System.Data.IDbConnection conn, long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(long customerTemplateScriptId);
        void CustomerTemplateScript_Update(PromoStudio.Common.Models.CustomerTemplateScript customerTemplateScript);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScriptItem> CustomerTemplateScriptItem_InsertAsync(PromoStudio.Common.Models.CustomerTemplateScriptItem customerTemplateScriptItem);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerTemplateScriptItem>> CustomerTemplateScriptItem_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScriptItem> CustomerTemplateScriptItem_SelectByIdAsync(long customerTemplateScriptItemId);
        void CustomerTemplateScriptItem_Update(PromoStudio.Common.Models.CustomerTemplateScriptItem customerTemplateScriptItem);
        void CustomerVideo_Delete(long customerVideoId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_InsertAsync(PromoStudio.Common.Models.CustomerVideo customerVideo);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_SelectAsync(long customerVideoId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_SelectAsyncWithItems(long customerVideoId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerVideo>> CustomerVideo_SelectByCustomerIdAsync(long customerId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerVideo>> CustomerVideo_SelectForProcessingAsync(int errorProcessingDelayInSeconds);
        void CustomerVideo_Update(PromoStudio.Common.Models.CustomerVideo customerVideo);
        void CustomerVideoItem_Delete(long customerVideoItemId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoItem> CustomerVideoItem_InsertAsync(PromoStudio.Common.Models.CustomerVideoItem customerVideoItem);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoItem> CustomerVideoItem_SelectAsync(long customerVideoItemId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerVideoItem>> CustomerVideoItem_SelectByCustomerVideoIdAsync(long customerVideoId);
        void CustomerVideoItem_Update(PromoStudio.Common.Models.CustomerVideoItem customerVideoItem);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoScript> CustomerVideoScript_InsertAsync(PromoStudio.Common.Models.CustomerVideoScript customerVideoScript);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoScript> CustomerVideoScript_SelectAsync(long customerVideoScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoScript> CustomerVideoScript_SelectByCustomerVideoIdAsync(long customerVideoId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoVoiceOver> CustomerVideoVoiceOver_InsertAsync(PromoStudio.Common.Models.CustomerVideoVoiceOver customerVideoVoiceOver);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectAsync(long customerVideoVoiceOverId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectByCustomerVideoIdAsync(long customerVideoId);
        void CustomerVideoVoiceOver_Update(PromoStudio.Common.Models.CustomerVideoVoiceOver customerVideoVoiceOver);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerWithLoginCredential>> CustomerWithLoginCredential_SelectAsyncByLoginCredential(sbyte customerLoginPlatformId, string loginKey, string emailAddress);
        IDataWrapper DataWrapper { get; set; }
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockAudio>> StockAudio_SelectAll();
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockAudio>> StockAudio_SelectByCustomerVideoId(long customerVideoId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockVideo>> StockVideo_SelectAll();
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockVideo>> StockVideo_SelectByCustomerVideoId(long customerVideoId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.TemplateScript>> TemplateScript_SelectAllWithItemsAsync();
        System.Threading.Tasks.Task<PromoStudio.Common.Models.TemplateScript> TemplateScript_SelectAsync(long templateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.TemplateScript> TemplateScript_SelectWithItemsAsync(long templateScriptId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.TemplateScriptItem>> TemplateScriptItem_SelectByTemplateScriptIdAsync(long templateScriptId);
    }
}
