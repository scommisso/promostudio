﻿using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PromoStudio.Common.Models;

namespace PromoStudio.Data
{
    public interface IDataService
    {
        void AudioScriptTemplate_Delete(int audioScriptTemplateId);
        Task<AudioScriptTemplate> AudioScriptTemplate_InsertAsync(AudioScriptTemplate audioScript);
        void AudioScriptTemplate_Update(AudioScriptTemplate audioScript);
        Task<AudioScriptTemplate> AudioScriptTemplate_SelectAsync(long audioScriptTemplateId);
        Task<AudioScriptTemplate> AudioScriptTemplate_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<IEnumerable<AudioScriptTemplate>> AudioScriptTemplate_SelectActiveByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId);
        Task<IEnumerable<AudioScriptTemplate>> AudioScriptTemplate_SelectAllAsync();
        Task<Customer> Customer_SelectAsync(long customerId);
        Task<Customer> Customer_InsertAsync(Customer customer);
        void Customer_Update(Customer customer);
        void Customer_Delete(long customerId);

        Task<IEnumerable<CustomerWithLoginCredential>> CustomerWithLoginCredential_SelectByLoginCredentialAsync(
            sbyte customerLoginPlatformId, string loginKey, string emailAddress);

        void CustomerLoginCredential_InsertUpdate(CustomerLoginCredential customerLoginCredential);
        Task<CustomerVideo> CustomerVideo_SelectAsync(long customerVideoId);
        Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectByCustomerIdAsync(long customerId);
        Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForProcessingAsync(int errorProcessingDelayInSeconds);
        Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForCloudStatusCheckAsync();
        Task<CustomerVideo> CustomerVideo_SelectWithItemsAsync(long customerVideoId);
        Task<CustomerVideo> CustomerVideo_InsertAsync(CustomerVideo customerVideo);
        void CustomerVideo_Update(CustomerVideo customerVideo);
        void CustomerVideo_Delete(long customerVideoId);
        Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectAsync(long customerVideoVoiceOverId);
        Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_InsertAsync(CustomerVideoVoiceOver customerVideoVoiceOver);
        void CustomerVideoVoiceOver_Update(CustomerVideoVoiceOver customerVideoVoiceOver);
        Task<CustomerVideoItem> CustomerVideoItem_SelectAsync(long customerVideoItemId);
        Task<IEnumerable<CustomerVideoItem>> CustomerVideoItem_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<CustomerVideoItem> CustomerVideoItem_InsertAsync(CustomerVideoItem customerVideoItem);
        void CustomerVideoItem_Update(CustomerVideoItem customerVideoItem);
        void CustomerVideoItem_Delete(long customerVideoItemId);
        Task<CustomerVideoScript> CustomerVideoScript_InsertAsync(CustomerVideoScript customerVideoScript);
        Task<CustomerVideoScript> CustomerVideoScript_SelectAsync(long customerVideoScriptId);
        Task<CustomerVideoScript> CustomerVideoScript_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<CustomerResource> CustomerResource_SelectAsync(long customerResourceId);
        Task<IEnumerable<CustomerResource>> CustomerResource_SelectActiveByCustomerIdAsync(long customerId);

        Task<IEnumerable<CustomerResource>> CustomerResource_SelectByCustomerTemplateScriptIdAsync(
            long customerTemplateScriptId);

        Task<CustomerResource> CustomerResource_InsertAsync(CustomerResource customerResource);
        void CustomerResource_Update(CustomerResource customerResource);
        void CustomerResource_Delete(long customerResourceId);
        Task<CustomerTemplateScript> CustomerTemplateScript_SelectAsync(long customerTemplateScriptId);
        Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(long customerTemplateScriptId);

        Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(IDbConnection conn,
            long customerTemplateScriptId);

        Task<CustomerTemplateScript> CustomerTemplateScript_InsertAsync(CustomerTemplateScript customerTemplateScript);
        void CustomerTemplateScript_Update(CustomerTemplateScript customerTemplateScript);
        Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_SelectByIdAsync(long customerTemplateScriptItemId);

        Task<IEnumerable<CustomerTemplateScriptItem>> CustomerTemplateScriptItem_SelectByCustomerTemplateScriptIdAsync(
            long customerTemplateScriptId);

        Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_InsertAsync(
            CustomerTemplateScriptItem customerTemplateScriptItem);

        void CustomerTemplateScriptItem_Update(CustomerTemplateScriptItem customerTemplateScriptItem);
        Task<Organization> Organization_SelectAsync(int organizationId);
        Task<IEnumerable<StockAudio>> StockAudio_SelectAllAsync();
        Task<IEnumerable<StockAudio>> StockAudio_SelectByCustomerVideoIdAsync(long customerVideoId);

        Task<IEnumerable<StockAudio>> StockAudio_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId,
            int? verticalId);

        Task<IEnumerable<StockVideo>> StockVideo_SelectAllAsync();
        Task<IEnumerable<StockVideo>> StockVideo_SelectByCustomerVideoIdAsync(long customerVideoId);

        Task<IEnumerable<StockVideo>> StockVideo_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId,
            int? verticalId);

        Task<IEnumerable<StockVideo>> StockVideo_SelectByStoryboardIdAsync(int storyboardId);
        void Storyboard_Delete(int storyboardId);
        Task<Storyboard> Storyboard_InsertAsync(Storyboard storyboard);
        Task<IEnumerable<Storyboard>> Storyboard_SelectAllAsync();
        Task<IEnumerable<Storyboard>> Storyboard_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<Storyboard> Storyboard_SelectAsync(int storyboardId);
        Task<Storyboard> Storyboard_SelectWithItemsAsync(int storyboardId);

        Task<IEnumerable<Storyboard>> Storyboard_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId,
            int? verticalId);

        void Storyboard_Update(Storyboard storyboard);
        Task<StoryboardItem> StoryboardItem_InsertAsync(StoryboardItem storyboardItem);
        void StoryboardItem_Delete(long storyboardItemId);
        Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByCustomerVideoIdAsync(long customerVideoId);
        Task<StoryboardItem> StoryboardItem_SelectByIdAsync(long storyboardItemId);
        Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByStoryboardIdAsync(int storyboardId);
        Task<IEnumerable<TemplateScript>> TemplateScript_SelectAllWithItemsAsync();
        Task<TemplateScript> TemplateScript_SelectAsync(long templateScriptId);

        Task<IEnumerable<TemplateScript>> TemplateScript_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId,
            int? verticalId);

        Task<TemplateScript> TemplateScript_SelectWithItemsAsync(long templateScriptId);
        Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdAsync(int storyboardId);
        Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdWithItemsAsync(int storyboardId);
        Task<IEnumerable<TemplateScriptItem>> TemplateScriptItem_SelectByTemplateScriptIdAsync(long templateScriptId);
        Task<IEnumerable<VoiceActor>> VoiceActor_SelectAllAsync();
    }
}