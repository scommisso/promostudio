using System;
namespace PromoStudio.Interfaces
{
    public interface IDataService
    {
        System.Threading.Tasks.Task Customer_DeleteAsync(long customerId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.Customer> Customer_InsertAsync(PromoStudio.Common.Models.Customer customer);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.Customer> Customer_SelectAsync(long customerId);
        System.Threading.Tasks.Task Customer_UpdateAsync(PromoStudio.Common.Models.Customer customer);
        System.Threading.Tasks.Task CustomerResource_DeleteAsync(long customerResourceId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerResource> CustomerResource_InsertAsync(PromoStudio.Common.Models.CustomerResource customerResource);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerResource>> CustomerResource_SelectActiveByCustomerIdAsync(long customerId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerResource>> CustomerResource_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_InsertAsync(PromoStudio.Common.Models.CustomerTemplateScript customerTemplateScript);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(System.Data.IDbConnection conn, long customerTemplateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(long customerTemplateScriptId);
        System.Threading.Tasks.Task CustomerTemplateScript_UpdateAsync(PromoStudio.Common.Models.CustomerTemplateScript customerTemplateScript);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerTemplateScriptItem> CustomerTemplateScriptItem_InsertAsync(PromoStudio.Common.Models.CustomerTemplateScriptItem customerTemplateScriptItem);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerTemplateScriptItem>> CustomerTemplateScriptItem_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptItemId);
        System.Threading.Tasks.Task CustomerTemplateScriptItem_UpdateAsync(PromoStudio.Common.Models.CustomerTemplateScriptItem customerTemplateScriptItem);
        System.Threading.Tasks.Task CustomerVideo_DeleteAsync(long customerVideoId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_InsertAsync(PromoStudio.Common.Models.CustomerVideo customerVideo);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_SelectAsync(long customerVideoId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideo> CustomerVideo_SelectAsyncWithItems(long customerVideoId);
        System.Threading.Tasks.Task CustomerVideo_UpdateStatusAsync(PromoStudio.Common.Models.CustomerVideo customerVideo);
        System.Threading.Tasks.Task CustomerVideoItem_DeleteAsync(long customerVideoItemId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoItem> CustomerVideoItem_InsertAsync(PromoStudio.Common.Models.CustomerVideoItem customerVideoItem);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.CustomerVideoItem>> CustomerVideoItem_SelectByCustomerVideoIdAsync(long customerVideoId);
        System.Threading.Tasks.Task CustomerVideoItem_UpdateAsync(PromoStudio.Common.Models.CustomerVideoItem customerVideoItem);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoVoiceOver> CustomerVideoVoiceOver_InsertAsync(PromoStudio.Common.Models.CustomerVideoVoiceOver customerVideoVoiceOver);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectByCustomerVideoIdAsync(long customerVideoId);
        System.Threading.Tasks.Task CustomerVideoVoiceOver_UpdateAsync(PromoStudio.Common.Models.CustomerVideoVoiceOver customerVideoVoiceOver);
        PromoStudio.Interfaces.IDataWrapper DataWrapper { get; set; }
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockAudio>> StockAudio_SelectByCustomerVideoId(long customerVideoId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.StockVideo>> StockVideo_SelectByCustomerVideoId(long customerVideoId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.TemplateScript>> TemplateScript_SelectAllWithItemsAsync();
        System.Threading.Tasks.Task<PromoStudio.Common.Models.TemplateScript> TemplateScript_SelectAsync(long templateScriptId);
        System.Threading.Tasks.Task<PromoStudio.Common.Models.TemplateScript> TemplateScript_SelectWithItemsAsync(long templateScriptId);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<PromoStudio.Common.Models.TemplateScriptItem>> TemplateScriptItem_SelectByTemplateScriptIdAsync(long templateScriptId);
    }
}
