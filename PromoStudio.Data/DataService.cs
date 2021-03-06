﻿using PromoStudio.Common;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PromoStudio.Data
{
    public class DataService : IDataService
    {
        #region Properties

        protected IDataWrapper DataWrapper { get; set; }

        #endregion

        #region Constructor

        public DataService(IDataWrapper dataWrapper)
        {
            if (dataWrapper == null)
                throw new ArgumentNullException("dataWrapper");
            DataWrapper = dataWrapper;
        }

        #endregion

        #region AudioScriptTemplate

        public void AudioScriptTemplate_Delete(int audioScriptTemplateId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.AudioScriptTemplateDelete_sp,
                dbParams: new { AudioScriptTemplateId = audioScriptTemplateId });
        }

        public async Task<AudioScriptTemplate> AudioScriptTemplate_InsertAsync(AudioScriptTemplate audioScript)
        {
            audioScript.DateCreated = DateTime.UtcNow;
            var result = ((IEnumerable<AudioScriptTemplate>)(await DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(Constants.StoredProcedures.AudioScriptTemplateInsert_sp,
                dbParams: audioScript.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void AudioScriptTemplate_Update(AudioScriptTemplate audioScript)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.AudioScriptTemplateUpdate_sp,
                dbParams: audioScript.ToPoco());
        }

        public async Task<AudioScriptTemplate> AudioScriptTemplate_SelectAsync(long audioScriptTemplateId)
        {
            var audioScript = ((IEnumerable<AudioScriptTemplate>)(await DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(Constants.StoredProcedures.AudioScriptTemplateSelectById_sp,
                dbParams: new { AudioScriptTemplateId = audioScriptTemplateId })))
                .FirstOrDefault();

            return audioScript;
        }

        public async Task<AudioScriptTemplate> AudioScriptTemplate_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var audioScript = ((IEnumerable<AudioScriptTemplate>)(await DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(Constants.StoredProcedures.AudioScriptTemplateSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId })))
                .FirstOrDefault();

            return audioScript;
        }

        public async Task<IEnumerable<AudioScriptTemplate>> AudioScriptTemplate_SelectAllAsync()
        {
            var audioScripts = (await DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(Constants.StoredProcedures.AudioScriptTemplateSelectAll_sp))
                .ToList();

            return audioScripts;
        }

        #endregion

        #region Customer

        public async Task<Customer> Customer_SelectAsync(long customerId)
        {
            var customer = ((IEnumerable<Customer>)(await DataWrapper.QueryStoredProcAsync<Customer>(Constants.StoredProcedures.CustomerSelectById_sp,
                dbParams: new { CustomerId = customerId })))
                .FirstOrDefault();

            return customer;
        }

        public async Task<Customer> Customer_InsertAsync(Customer customer)
        {
            customer.DateCreated = DateTime.UtcNow;
            var result =
                ((IEnumerable<Customer>)await DataWrapper.QueryStoredProcAsync<Customer>(Constants.StoredProcedures.CustomerInsert_sp,
                dbParams: customer.ToPoco()))
                .FirstOrDefault();

            return result;
        }

        public void Customer_Update(Customer customer)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerUpdate_sp,
                dbParams: customer.ToPoco());
        }

        public void Customer_Delete(long customerId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerDelete_sp,
                dbParams: new { CustomerId = customerId });
        }

        #endregion

        #region CustomerLoginCredential

        public async Task<IEnumerable<CustomerWithLoginCredential>> CustomerWithLoginCredential_SelectByLoginCredentialAsync(sbyte customerLoginPlatformId, string loginKey, string emailAddress)
        {
            var customerLogins = (await DataWrapper.QueryStoredProcAsync<CustomerWithLoginCredential>(Constants.StoredProcedures.CustomerSelectByLoginCredential_sp,
                dbParams: new { CustomerLoginPlatformId = customerLoginPlatformId, LoginKey = loginKey, EmailAddress = emailAddress }))
                .ToList();

            return customerLogins;
        }

        public void CustomerLoginCredential_InsertUpdate(CustomerLoginCredential customerLoginCredential)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerLoginCredentialInsertUpdate_sp,
                dbParams: customerLoginCredential.ToPoco());
        }

        #endregion

        #region CustomerVideo

        public async Task<CustomerVideo> CustomerVideo_SelectAsync(long customerVideoId)
        {
            var customerVideo = ((IEnumerable<CustomerVideo>)(await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoSelectById_sp,
                dbParams: new { CustomerVideoId = customerVideoId })))
                .FirstOrDefault();

            return customerVideo;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectByCustomerIdAsync(long customerId)
        {
            var customerVideos = (await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoSelectByCustomerId_sp,
                dbParams: new { CustomerId = customerId }))
                .ToList();

            return customerVideos;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForProcessingAsync(int errorProcessingDelayInSeconds)
        {
            var customerVideos = (await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoSelectForProcessing_sp,
                    dbParams: new { ErrorDelaySeconds = errorProcessingDelayInSeconds }))
                .ToList();

            return customerVideos;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForCloudStatusCheckAsync()
        {
            var customerVideos = (await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoSelectForCloudStatusCheck_sp))
                .ToList();

            return customerVideos;
        }

        public async Task<CustomerVideo> CustomerVideo_SelectWithItemsAsync(long customerVideoId)
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                var customerVideo = ((IEnumerable<CustomerVideo>)(await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoSelectById_sp,
                    dbParams: new { CustomerVideoId = customerVideoId })))
                    .FirstOrDefault();
                if (customerVideo == null) { return null; }

                var items = (await DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(conn, Constants.StoredProcedures.CustomerVideoItemSelectByCustomerVideoId_sp,
                    dbParams: new { CustomerVideoId = customerVideoId }))
                    .ToList();

                IEnumerable<StockVideo> stockVideo = new StockVideo[0];
                IEnumerable<StockAudio> stockAudio = new StockAudio[0];
                IEnumerable<CustomerVideoVoiceOver> voiceOver = new CustomerVideoVoiceOver[0];

                var templateTasks = new List<Task<CustomerTemplateScript>>();
                Task<IEnumerable<StockVideo>> stockVideoTask = null;
                Task<IEnumerable<StockAudio>> stockAudioTask = null;
                Task<IEnumerable<CustomerVideoVoiceOver>> voiceOverTask = null;
                var tasks = new List<Task>();

                templateTasks.AddRange(items
                    .Where(cvi => cvi.Type == CustomerVideoItemType.CustomerTemplateScript)
                    .Select(cvi => CustomerTemplateScript_SelectWithItemsAsync(conn, cvi.fk_CustomerVideoItemId)));
                tasks.AddRange(templateTasks);

                if (items.Any(cvi => cvi.Type == CustomerVideoItemType.StockVideo))
                {
                    stockVideoTask = DataWrapper.QueryStoredProcAsync<StockVideo>(conn,
                        Constants.StoredProcedures.StockVideoSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId});
                    tasks.Add(stockVideoTask);
                }
                if (items.Any(cvi => cvi.Type == CustomerVideoItemType.StockAudio))
                {
                    stockAudioTask = DataWrapper.QueryStoredProcAsync<StockAudio>(conn,
                        Constants.StoredProcedures.StockAudioSelectByCustomerVideoId_sp,
                        dbParams: new { CustomerVideoId = customerVideoId });
                    tasks.Add(stockAudioTask);
                }
                if (items.Any(cvi => cvi.Type == CustomerVideoItemType.CustomerVideoVoiceOver))
                {
                    voiceOverTask = DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(conn,
                        Constants.StoredProcedures.CustomerVideoVoiceOverSelectByCustomerVideoId_sp,
                        dbParams: new { CustomerVideoId = customerVideoId });
                    tasks.Add(voiceOverTask);
                }

                await Task.WhenAll(tasks);

                var templates = templateTasks.Select(tt => tt.Result).ToList();
                if (stockVideoTask != null)
                {
                    stockVideo = stockVideoTask.Result.ToList();
                }
                if (stockAudioTask != null)
                {
                    stockAudio = stockAudioTask.Result.ToList();
                }
                if (voiceOverTask != null)
                {
                    voiceOver = voiceOverTask.Result.ToList();
                }

                foreach (var item in items)
                {
                    if (item.Type == CustomerVideoItemType.CustomerTemplateScript)
                    {
                        item.CustomerScript = templates.FirstOrDefault(t => t.pk_CustomerTemplateScriptId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.CustomerVideoVoiceOver) {
                        item.VoiceOver = voiceOver.FirstOrDefault(vo => vo.pk_CustomerVideoVoiceOverId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.StockVideo)
                    {
                        item.StockVideo = stockVideo.FirstOrDefault(sv => sv.pk_StockVideoId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.StockAudio)
                    {
                        item.StockAudio = stockAudio.FirstOrDefault(sa => sa.pk_StockAudioId == item.fk_CustomerVideoItemId);
                    }
                }

                customerVideo.Items.AddRange(items);
                return customerVideo;
            }
        }

        public async Task<CustomerVideo> CustomerVideo_InsertAsync(CustomerVideo customerVideo)
        {
            customerVideo.DateCreated = DateTime.UtcNow;
            var result =
                ((IEnumerable<CustomerVideo>)(await DataWrapper.QueryStoredProcAsync<CustomerVideo>(Constants.StoredProcedures.CustomerVideoInsert_sp,
                dbParams: customerVideo.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerVideo_Update(CustomerVideo customerVideo)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerVideoUpdate_sp,
                dbParams: customerVideo.ToPoco());
        }

        public void CustomerVideo_Delete(long customerVideoId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerVideoDelete_sp,
                dbParams: new { CustomerVideoId = customerVideoId });
        }

        #endregion

        #region CustomerVideoVoiceOver

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectAsync(long customerVideoVoiceOverId)
        {
            var customerVideoVoiceOver = ((IEnumerable<CustomerVideoVoiceOver>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(Constants.StoredProcedures.CustomerVideoVoiceOverSelectById_sp,
                dbParams: new { CustomerVideoVoiceOverId = customerVideoVoiceOverId })))
                .FirstOrDefault();

            return customerVideoVoiceOver;
        }

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var customerVideoVoiceOver = ((IEnumerable<CustomerVideoVoiceOver>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(Constants.StoredProcedures.CustomerVideoVoiceOverSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId })))
                .FirstOrDefault();

            return customerVideoVoiceOver;
        }

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_InsertAsync(CustomerVideoVoiceOver customerVideoVoiceOver)
        {
            customerVideoVoiceOver.DateCreated = DateTime.UtcNow;
            var result =
                ((IEnumerable<CustomerVideoVoiceOver>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(Constants.StoredProcedures.CustomerVideoVoiceOverInsert_sp,
                dbParams: customerVideoVoiceOver.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerVideoVoiceOver_Update(CustomerVideoVoiceOver customerVideoVoiceOver)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerVideoVoiceOverUpdate_sp,
                dbParams: customerVideoVoiceOver.ToPoco());
        }

        #endregion

        #region CustomerVideoItem

        public async Task<CustomerVideoItem> CustomerVideoItem_SelectAsync(long customerVideoItemId)
        {
            var customerVideoItem = ((IEnumerable<CustomerVideoItem>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(Constants.StoredProcedures.CustomerVideoItemSelectById_sp,
                dbParams: new { CustomerVideoItemId = customerVideoItemId })))
                .FirstOrDefault();

            return customerVideoItem;
        }

        public async Task<IEnumerable<CustomerVideoItem>> CustomerVideoItem_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var customerVideoItems = (await DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(Constants.StoredProcedures.CustomerVideoItemSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId }))
                .ToList();

            return customerVideoItems;
        }

        public async Task<CustomerVideoItem> CustomerVideoItem_InsertAsync(CustomerVideoItem customerVideoItem)
        {
            var result =
                ((IEnumerable<CustomerVideoItem>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(Constants.StoredProcedures.CustomerVideoItemInsert_sp,
                dbParams: customerVideoItem.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerVideoItem_Update(CustomerVideoItem customerVideoItem)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerVideoItemUpdate_sp,
                dbParams: customerVideoItem.ToPoco());
        }

        public void CustomerVideoItem_Delete(long customerVideoItemId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerVideoItemDelete_sp,
                dbParams: new { CustomerVideoItemId = customerVideoItemId });
        }

        #endregion

        #region CustomerVideoScript

        public async Task<CustomerVideoScript> CustomerVideoScript_InsertAsync(CustomerVideoScript customerVideoScript)
        {
            var result = ((IEnumerable<CustomerVideoScript>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(Constants.StoredProcedures.CustomerVideoScriptInsert_sp,
                dbParams: customerVideoScript.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public async Task<CustomerVideoScript> CustomerVideoScript_SelectAsync(long customerVideoScriptId)
        {
            var audioScript = ((IEnumerable<CustomerVideoScript>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(Constants.StoredProcedures.AudioScriptTemplateSelectById_sp,
                dbParams: new { CustomerVideoScriptId = customerVideoScriptId })))
                .FirstOrDefault();

            return audioScript;
        }

        public async Task<CustomerVideoScript> CustomerVideoScript_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var audioScript = ((IEnumerable<CustomerVideoScript>)(await DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(Constants.StoredProcedures.AudioScriptTemplateSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId })))
                .FirstOrDefault();

            return audioScript;
        }

        #endregion

        #region CustomerResource

        public async Task<CustomerResource> CustomerResource_SelectAsync(long customerResourceId)
        {
            var resource = ((IEnumerable<CustomerResource>)(await DataWrapper.QueryStoredProcAsync<CustomerResource>(Constants.StoredProcedures.CustomerResourceSelectById_sp,
                dbParams: new { CustomerResourceId = customerResourceId })))
                .FirstOrDefault();

            return resource;
        }

        public async Task<IEnumerable<CustomerResource>> CustomerResource_SelectActiveByCustomerIdAsync(long customerId)
        {
            var resources = (await DataWrapper.QueryStoredProcAsync<CustomerResource>(Constants.StoredProcedures.CustomerResourceSelectActiveByCustomerId_sp,
                dbParams: new { CustomerId = customerId }))
                .ToList();

            return resources;
        }

        public async Task<IEnumerable<CustomerResource>> CustomerResource_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId)
        {
            var resources = (await DataWrapper.QueryStoredProcAsync<CustomerResource>(Constants.StoredProcedures.CustomerResourceSelectByCustomerTemplateScriptId_sp,
                dbParams: new { CustomerTemplateScriptId = customerTemplateScriptId }))
                .ToList();

            return resources;
        }

        public async Task<CustomerResource> CustomerResource_InsertAsync(CustomerResource customerResource)
        {
            customerResource.DateCreated = DateTime.UtcNow;
            var result =
                ((IEnumerable<CustomerResource>)(await DataWrapper.QueryStoredProcAsync<CustomerResource>(Constants.StoredProcedures.CustomerResourceInsert_sp,
                dbParams: customerResource.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerResource_Delete(long customerResourceId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerResourceDelete_sp,
                dbParams: new { CustomerResourceId = customerResourceId });
        }

        #endregion

        #region CustomerTemplateScript

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectAsync(long customerTemplateScriptId)
        {
            var script = ((IEnumerable<CustomerTemplateScript>)(await DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(Constants.StoredProcedures.CustomerTemplateScriptSelectById_sp,
                dbParams: new { CustomerTemplateScriptId = customerTemplateScriptId })))
                .FirstOrDefault();

            return script;
        }

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(long customerTemplateScriptId)
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                return await CustomerTemplateScript_SelectWithItemsAsync(conn, customerTemplateScriptId);
            }
        }

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(IDbConnection conn, long customerTemplateScriptId)
        {
            var script = ((IEnumerable<CustomerTemplateScript>)(await DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(conn, Constants.StoredProcedures.CustomerTemplateScriptSelectById_sp,
                dbParams: new { CustomerTemplateScriptId = customerTemplateScriptId })))
                .FirstOrDefault();
            if (script == null) { return null; }

            var templateTask =
                DataWrapper.QueryStoredProcAsync<TemplateScript>(Constants.StoredProcedures.TemplateScriptSelectById_sp,
                    dbParams: new {TemplateScriptId = script.fk_TemplateScriptId});
            var templateItemsTask = DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                dbParams: new {TemplateScriptId = script.fk_TemplateScriptId});
            var itemsTask = DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(conn,
                Constants.StoredProcedures.CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp,
                dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId});
            var resourcesTask = DataWrapper.QueryStoredProcAsync<CustomerResource>(conn,
                Constants.StoredProcedures.CustomerResourceSelectByCustomerTemplateScriptId_sp,
                dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId});

            await Task.WhenAll(templateTask, templateItemsTask, itemsTask, resourcesTask);

            var template = ((IEnumerable<TemplateScript>)templateTask.Result).FirstOrDefault();
            var templateItems = templateItemsTask.Result.ToList();
            var items = itemsTask.Result.ToList();
            var resources = resourcesTask.Result.ToList();

            // Map resource to item
            foreach (var item in items)
            {
                item.Resource = resources.FirstOrDefault(r => r.pk_CustomerResourceId == item.fk_CustomerResourceId);
                item.ScriptItem = templateItems.FirstOrDefault(ti => ti.pk_TemplateScriptItemId == item.fk_TemplateScriptItemId);
            }

            // Map items to script
            script.Items.AddRange(items);
            script.Template = template;
            if (script.Template != null)
            {
                script.Template.Items.AddRange(templateItems);
            }

            return script;
        }

        public async Task<CustomerTemplateScript> CustomerTemplateScript_InsertAsync(CustomerTemplateScript customerTemplateScript)
        {
            customerTemplateScript.DateCreated = DateTime.UtcNow;
            var result =
                ((IEnumerable<CustomerTemplateScript>)(await DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(Constants.StoredProcedures.CustomerTemplateScriptInsert_sp,
                dbParams: customerTemplateScript.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerTemplateScript_Update(CustomerTemplateScript customerTemplateScript)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerTemplateScriptUpdate_sp,
                dbParams: customerTemplateScript.ToPoco());
        }

        #endregion

        #region CustomerTemplateScriptItem

        public async Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_SelectByIdAsync(long customerTemplateScriptItemId)
        {
            var scriptItem = ((IEnumerable<CustomerTemplateScriptItem>)(await DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(Constants.StoredProcedures.CustomerTemplateScriptItemSelectById_sp,
                dbParams: new { CustomerTemplateScriptItemId = customerTemplateScriptItemId })))
                .FirstOrDefault();

            return scriptItem;
        }

        public async Task<IEnumerable<CustomerTemplateScriptItem>> CustomerTemplateScriptItem_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId)
        {
            var scriptItems = (await DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(Constants.StoredProcedures.CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp,
                dbParams: new { CustomerTemplateScriptId = customerTemplateScriptId }))
                .ToList();

            return scriptItems;
        }

        public async Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_InsertAsync(CustomerTemplateScriptItem customerTemplateScriptItem)
        {
            var result =
                ((IEnumerable<CustomerTemplateScriptItem>)(await DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(Constants.StoredProcedures.CustomerTemplateScriptItemInsert_sp,
                dbParams: customerTemplateScriptItem.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void CustomerTemplateScriptItem_Update(CustomerTemplateScriptItem customerTemplateScriptItem)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerTemplateScriptItemUpdate_sp,
                dbParams: customerTemplateScriptItem.ToPoco());
        }

        #endregion

        #region Organization

        public async Task<Organization> Organization_SelectAsync(int organizationId)
        {
            var organization = ((IEnumerable<Organization>)(await DataWrapper.QueryStoredProcAsync<Organization>(Constants.StoredProcedures.OrganizationSelectById_sp,
                dbParams: new { OrganizationId = organizationId })))
                .FirstOrDefault();

            return organization;
        }

        #endregion

        #region StockAudio

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectAllAsync()
        {
            var stockAudio = (await DataWrapper.QueryStoredProcAsync<StockAudio>(Constants.StoredProcedures.StockAudioSelectAll_sp))
                .ToList();
            return stockAudio;
        }

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var stockAudio = (await DataWrapper.QueryStoredProcAsync<StockAudio>(Constants.StoredProcedures.StockAudioSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId }))
                .ToList();
            return stockAudio;
        }

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId)
        {
            var stockAudio = (await DataWrapper.QueryStoredProcAsync<StockAudio>(Constants.StoredProcedures.StockAudioSelectByOrganizationIdAndVerticalId_sp,
                dbParams: new {
                    OrganizationId = organizationId,
                    VerticalId = verticalId
                }))
                .ToList();
            return stockAudio;
        }

        #endregion

        #region StockVideo

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectAllAsync()
        {
            var stockVideo = (await DataWrapper.QueryStoredProcAsync<StockVideo>(Constants.StoredProcedures.StockVideoSelectAll_sp))
                .ToList();
            return stockVideo;
        }

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var stockVideo = (await DataWrapper.QueryStoredProcAsync<StockVideo>(Constants.StoredProcedures.StockVideoSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId }))
                .ToList();
            return stockVideo;
        }

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId)
        {
            var stockVideo = (await DataWrapper.QueryStoredProcAsync<StockVideo>(Constants.StoredProcedures.StockVideoSelectByOrganizationIdAndVerticalId_sp,
                dbParams: new
                {
                    OrganizationId = organizationId,
                    VerticalId = verticalId
                }))
                .ToList();
            return stockVideo;
        }

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectByStoryboardIdAsync(int storyboardId)
        {
            var stockVideo = (await DataWrapper.QueryStoredProcAsync<StockVideo>(Constants.StoredProcedures.StockVideoSelectByStoryboardId_sp,
                dbParams: new { StoryboardId = storyboardId }))
                .ToList();
            return stockVideo;
        }

        #endregion

        #region Storyboard

        public void Storyboard_Delete(int storyboardId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.StoryboardDelete_sp,
                dbParams: new { StoryboardId = storyboardId });
        }

        public async Task<Storyboard> Storyboard_InsertAsync(Storyboard storyboard)
        {
            storyboard.DateCreated = DateTime.UtcNow;
            var newStoryboard = ((IEnumerable<Storyboard>)(await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardInsert_sp,
                dbParams: storyboard.ToPoco())))
                .FirstOrDefault();
            return newStoryboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectAllAsync()
        {
            var storyboard = (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectAll_sp))
                .ToList();
            return storyboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var storyboard = (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId }))
                .ToList();
            return storyboard;
        }

        public async Task<Storyboard> Storyboard_SelectAsync(int storyboardId)
        {
            var storyboard = ((IEnumerable<Storyboard>)(await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectById_sp,
                dbParams: new { StoryboardId = storyboardId })))
                .FirstOrDefault();
            return storyboard;
        }

        public async Task<Storyboard> Storyboard_SelectWithItemsAsync(int storyboardId)
        {
            var storyboardTask =
                DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectById_sp,
                    dbParams: new {StoryboardId = storyboardId});
            var itemsTask =
                DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                    Constants.StoredProcedures.StoryboardItemSelectByStoryboardId_sp,
                    dbParams: new {StoryboardId = storyboardId});

            await Task.WhenAll(storyboardTask, itemsTask);

            var storyboard = ((IEnumerable<Storyboard>)storyboardTask.Result).FirstOrDefault();
            if (storyboard != null)
            {
                var items = itemsTask.Result.ToList();
                storyboard.Items = items;
            }


            return storyboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId)
        {
            var storyboard = (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectByOrganizationIdAndVerticalId_sp,
                dbParams: new
                {
                    OrganizationId = organizationId,
                    VerticalId = verticalId
                }))
                .ToList();
            return storyboard;
        }

        public void Storyboard_Update(Storyboard storyboard)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.StoryboardUpdate_sp,
                dbParams: storyboard.ToPoco());
        }

        #endregion

        #region StoryboardItem

        public async Task<StoryboardItem> StoryboardItem_InsertAsync(StoryboardItem storyboardItem)
        {
            var result =
                ((IEnumerable<StoryboardItem>)(await DataWrapper.QueryStoredProcAsync<StoryboardItem>(Constants.StoredProcedures.StoryboardItemInsert_sp,
                dbParams: storyboardItem.ToPoco())))
                .FirstOrDefault();

            return result;
        }

        public void StoryboardItem_Delete(long storyboardItemId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.StoryboardItemDelete_sp,
                dbParams: new { StoryboardItemId = storyboardItemId });
        }

        public async Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            var storyboardItems = (await DataWrapper.QueryStoredProcAsync<StoryboardItem>(Constants.StoredProcedures.StoryboardItemSelectByCustomerVideoId_sp,
                dbParams: new { CustomerVideoId = customerVideoId }))
                .ToList();
            return storyboardItems;
        }

        public async Task<StoryboardItem> StoryboardItem_SelectByIdAsync(long storyboardItemId)
        {
            var storyboardItem = ((IEnumerable<StoryboardItem>)(await DataWrapper.QueryStoredProcAsync<StoryboardItem>(Constants.StoredProcedures.StoryboardItemSelectById_sp,
                dbParams: new { StoryboardItemId = storyboardItemId })))
                .FirstOrDefault();
            return storyboardItem;
        }

        public async Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByStoryboardIdAsync(int storyboardId)
        {
            var storyboardItems = (await DataWrapper.QueryStoredProcAsync<StoryboardItem>(Constants.StoredProcedures.StoryboardItemSelectByStoryboardId_sp,
                dbParams: new { StoryboardId = storyboardId }))
                .ToList();
            return storyboardItems;
        }

        #endregion

        #region TemplateScript

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectAllWithItemsAsync()
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                var scriptsTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn, Constants.StoredProcedures.TemplateScriptSelectAll_sp);
                var itemsTask = DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn, Constants.StoredProcedures.TemplateScriptItemSelectAll_sp);

                await Task.WhenAll(scriptsTask, itemsTask);

                var scripts = scriptsTask.Result.ToList();
                var items = itemsTask.Result.ToList();

                foreach (var script in scripts)
                {
                    script.Items.AddRange(
                        items.Where(i => i.fk_TemplateScriptId == script.pk_TemplateScriptId));
                }
                return scripts;
            }
        }

        public async Task<TemplateScript> TemplateScript_SelectAsync(long templateScriptId)
        {
            var script = ((IEnumerable<TemplateScript>)(await DataWrapper.QueryStoredProcAsync<TemplateScript>(Constants.StoredProcedures.TemplateScriptSelectById_sp,
                dbParams: new { TemplateScriptId = templateScriptId })))
                .FirstOrDefault();

            return script;
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId)
        {
            var templateScripts = (await DataWrapper.QueryStoredProcAsync<TemplateScript>(Constants.StoredProcedures.TemplateScriptSelectByOrganizationIdAndVerticalId_sp,
                dbParams: new
                {
                    OrganizationId = organizationId,
                    VerticalId = verticalId
                }))
                .ToList();
            return templateScripts;
        }

        public async Task<TemplateScript> TemplateScript_SelectWithItemsAsync(long templateScriptId)
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                var scriptTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectById_sp,
                    dbParams: new {TemplateScriptId = templateScriptId});
                var itemsTask = DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                    Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                    dbParams: new {TemplateScriptId = templateScriptId});

                await Task.WhenAll(scriptTask, itemsTask);

                var script = ((IEnumerable<TemplateScript>)scriptTask.Result).FirstOrDefault();

                if (script != null)
                {
                    var items = itemsTask.Result.ToList();
                    script.Items.AddRange(items);
                }
                return script;
            }
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdAsync(int storyboardId)
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                var templateScripts = (await DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectByStoryboardId_sp,
                    dbParams: new {StoryboardId = storyboardId})
                    ).ToList();

                return templateScripts;
            }
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdWithItemsAsync(int storyboardId)
        {
            using (var conn = DataWrapper.OpenConnection())
            {
                var scriptsTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectByStoryboardId_sp,
                    dbParams: new { StoryboardId = storyboardId });
                var itemsTask = DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                    Constants.StoredProcedures.TemplateScriptItemSelectByStoryboardId_sp,
                    dbParams: new { StoryboardId = storyboardId });

                await Task.WhenAll(scriptsTask, itemsTask);

                var scripts = scriptsTask.Result.ToList();
                var items = itemsTask.Result.ToList();

                foreach (var script in scripts)
                {
                    var matchingItems =
                        items.Where(tsi => tsi.fk_TemplateScriptId == script.pk_TemplateScriptId)
                        .ToList();
                    script.Items.AddRange(matchingItems);
                }
                return scripts;
            }
        }

        #endregion

        #region TemplateScriptItem

        public async Task<IEnumerable<TemplateScriptItem>> TemplateScriptItem_SelectByTemplateScriptIdAsync(long templateScriptId)
        {
            var items = (await DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                        dbParams: new { TemplateScriptId = templateScriptId }))
                        .ToList();
            return items;
        }

        #endregion

        #region VoiceActor

        public async Task<IEnumerable<VoiceActor>> VoiceActor_SelectAllAsync()
        {
            var voiceActors = (await DataWrapper.QueryStoredProcAsync<VoiceActor>(Constants.StoredProcedures.VoiceActorSelectAll_sp))
                .ToList();
            return voiceActors;
        }

        #endregion
    }
}
