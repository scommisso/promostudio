using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PromoStudio.Common;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;

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
                dbParams: new {AudioScriptTemplateId = audioScriptTemplateId});
        }

        public async Task<AudioScriptTemplate> AudioScriptTemplate_InsertAsync(AudioScriptTemplate audioScript)
        {
            audioScript.DateCreated = DateTime.UtcNow;
            AudioScriptTemplate result =
                ((IEnumerable<AudioScriptTemplate>)
                    (await
                        DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(
                            Constants.StoredProcedures.AudioScriptTemplateInsert_sp,
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
            AudioScriptTemplate audioScript =
                (await
                    DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectById_sp,
                        dbParams: new {AudioScriptTemplateId = audioScriptTemplateId}))
                    .FirstOrDefault();

            return audioScript;
        }

        public async Task<AudioScriptTemplate> AudioScriptTemplate_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            AudioScriptTemplate audioScript =
                (await
                    DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .FirstOrDefault();

            return audioScript;
        }

        public async Task<IEnumerable<AudioScriptTemplate>> AudioScriptTemplate_SelectActiveByOrganizationIdAndVerticalIdAsync(int? organizationId, int? verticalId)
        {
            List<AudioScriptTemplate> audioScripts =
                (await
                    DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectActiveByOrganizationIdAndVerticalId_sp,
                        dbParams: new { OrganizationId = organizationId, VerticalId = verticalId }))
                    .ToList();

            return audioScripts;
        }

        public async Task<IEnumerable<AudioScriptTemplate>> AudioScriptTemplate_SelectAllAsync()
        {
            List<AudioScriptTemplate> audioScripts =
                (await
                    DataWrapper.QueryStoredProcAsync<AudioScriptTemplate>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectAll_sp))
                    .ToList();

            return audioScripts;
        }

        #endregion

        #region Customer

        public async Task<Customer> Customer_SelectAsync(long customerId)
        {
            Customer customer =
                (await DataWrapper.QueryStoredProcAsync<Customer>(Constants.StoredProcedures.CustomerSelectById_sp,
                    dbParams: new {CustomerId = customerId}))
                    .FirstOrDefault();

            return customer;
        }

        public async Task<Customer> Customer_InsertAsync(Customer customer)
        {
            customer.DateCreated = DateTime.UtcNow;
            Customer result =
                ((IEnumerable<Customer>)
                    await DataWrapper.QueryStoredProcAsync<Customer>(Constants.StoredProcedures.CustomerInsert_sp,
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
                dbParams: new {CustomerId = customerId});
        }

        #endregion

        #region CustomerLoginCredential

        public async Task<IEnumerable<CustomerWithLoginCredential>>
            CustomerWithLoginCredential_SelectByLoginCredentialAsync(sbyte customerLoginPlatformId, string loginKey,
                string emailAddress)
        {
            List<CustomerWithLoginCredential> customerLogins =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerWithLoginCredential>(
                        Constants.StoredProcedures.CustomerSelectByLoginCredential_sp,
                        dbParams:
                            new
                            {
                                CustomerLoginPlatformId = customerLoginPlatformId,
                                LoginKey = loginKey,
                                EmailAddress = emailAddress
                            }))
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
            CustomerVideo customerVideo =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                        Constants.StoredProcedures.CustomerVideoSelectById_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .FirstOrDefault();

            return customerVideo;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectByCustomerIdAsync(long customerId)
        {
            List<CustomerVideo> customerVideos =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                        Constants.StoredProcedures.CustomerVideoSelectByCustomerId_sp,
                        dbParams: new {CustomerId = customerId}))
                    .ToList();

            return customerVideos;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForProcessingAsync(
            int errorProcessingDelayInSeconds)
        {
            List<CustomerVideo> customerVideos =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                        Constants.StoredProcedures.CustomerVideoSelectForProcessing_sp,
                        dbParams: new {ErrorDelaySeconds = errorProcessingDelayInSeconds}))
                    .ToList();

            return customerVideos;
        }

        public async Task<IEnumerable<CustomerVideo>> CustomerVideo_SelectForCloudStatusCheckAsync()
        {
            List<CustomerVideo> customerVideos =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                        Constants.StoredProcedures.CustomerVideoSelectForCloudStatusCheck_sp))
                    .ToList();

            return customerVideos;
        }

        public async Task<CustomerVideo> CustomerVideo_SelectWithItemsAsync(long customerVideoId)
        {
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                CustomerVideo customerVideo =
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                            Constants.StoredProcedures.CustomerVideoSelectById_sp,
                            dbParams: new {CustomerVideoId = customerVideoId}))
                        .FirstOrDefault();
                if (customerVideo == null)
                {
                    return null;
                }

                List<CustomerVideoItem> items =
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(conn,
                            Constants.StoredProcedures.CustomerVideoItemSelectByCustomerVideoId_sp,
                            dbParams: new {CustomerVideoId = customerVideoId}))
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
                        dbParams: new {CustomerVideoId = customerVideoId});
                    tasks.Add(stockAudioTask);
                }
                if (items.Any(cvi => cvi.Type == CustomerVideoItemType.CustomerVideoVoiceOver))
                {
                    voiceOverTask = DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(conn,
                        Constants.StoredProcedures.CustomerVideoVoiceOverSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId});
                    tasks.Add(voiceOverTask);
                }

                await Task.WhenAll(tasks);

                List<CustomerTemplateScript> templates = templateTasks.Select(tt => tt.Result).ToList();
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

                foreach (CustomerVideoItem item in items)
                {
                    if (item.Type == CustomerVideoItemType.CustomerTemplateScript)
                    {
                        item.CustomerScript =
                            templates.FirstOrDefault(t => t.pk_CustomerTemplateScriptId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.CustomerVideoVoiceOver)
                    {
                        item.VoiceOver =
                            voiceOver.FirstOrDefault(vo => vo.pk_CustomerVideoVoiceOverId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.StockVideo)
                    {
                        item.StockVideo =
                            stockVideo.FirstOrDefault(sv => sv.pk_StockVideoId == item.fk_CustomerVideoItemId);
                    }
                    else if (item.Type == CustomerVideoItemType.StockAudio)
                    {
                        item.StockAudio =
                            stockAudio.FirstOrDefault(sa => sa.pk_StockAudioId == item.fk_CustomerVideoItemId);
                    }
                }

                customerVideo.Items.AddRange(items);
                return customerVideo;
            }
        }

        public async Task<CustomerVideo> CustomerVideo_InsertAsync(CustomerVideo customerVideo)
        {
            customerVideo.DateCreated = DateTime.UtcNow;
            CustomerVideo result =
                ((IEnumerable<CustomerVideo>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideo>(
                            Constants.StoredProcedures.CustomerVideoInsert_sp,
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
                dbParams: new {CustomerVideoId = customerVideoId});
        }

        #endregion

        #region CustomerVideoVoiceOver

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectAsync(long customerVideoVoiceOverId)
        {
            CustomerVideoVoiceOver customerVideoVoiceOver =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(
                        Constants.StoredProcedures.CustomerVideoVoiceOverSelectById_sp,
                        dbParams: new {CustomerVideoVoiceOverId = customerVideoVoiceOverId}))
                    .FirstOrDefault();

            return customerVideoVoiceOver;
        }

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_SelectByCustomerVideoIdAsync(
            long customerVideoId)
        {
            CustomerVideoVoiceOver customerVideoVoiceOver =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(
                        Constants.StoredProcedures.CustomerVideoVoiceOverSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .FirstOrDefault();

            return customerVideoVoiceOver;
        }

        public async Task<CustomerVideoVoiceOver> CustomerVideoVoiceOver_InsertAsync(
            CustomerVideoVoiceOver customerVideoVoiceOver)
        {
            customerVideoVoiceOver.DateCreated = DateTime.UtcNow;
            CustomerVideoVoiceOver result =
                ((IEnumerable<CustomerVideoVoiceOver>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideoVoiceOver>(
                            Constants.StoredProcedures.CustomerVideoVoiceOverInsert_sp,
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
            CustomerVideoItem customerVideoItem =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(
                        Constants.StoredProcedures.CustomerVideoItemSelectById_sp,
                        dbParams: new {CustomerVideoItemId = customerVideoItemId}))
                    .FirstOrDefault();

            return customerVideoItem;
        }

        public async Task<IEnumerable<CustomerVideoItem>> CustomerVideoItem_SelectByCustomerVideoIdAsync(
            long customerVideoId)
        {
            List<CustomerVideoItem> customerVideoItems =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(
                        Constants.StoredProcedures.CustomerVideoItemSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .ToList();

            return customerVideoItems;
        }

        public async Task<CustomerVideoItem> CustomerVideoItem_InsertAsync(CustomerVideoItem customerVideoItem)
        {
            CustomerVideoItem result =
                ((IEnumerable<CustomerVideoItem>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideoItem>(
                            Constants.StoredProcedures.CustomerVideoItemInsert_sp,
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
                dbParams: new {CustomerVideoItemId = customerVideoItemId});
        }

        #endregion

        #region CustomerVideoScript

        public async Task<CustomerVideoScript> CustomerVideoScript_InsertAsync(CustomerVideoScript customerVideoScript)
        {
            CustomerVideoScript result =
                ((IEnumerable<CustomerVideoScript>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(
                            Constants.StoredProcedures.CustomerVideoScriptInsert_sp,
                            dbParams: customerVideoScript.ToPoco())))
                    .FirstOrDefault();

            return result;
        }

        public async Task<CustomerVideoScript> CustomerVideoScript_SelectAsync(long customerVideoScriptId)
        {
            CustomerVideoScript audioScript =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectById_sp,
                        dbParams: new {CustomerVideoScriptId = customerVideoScriptId}))
                    .FirstOrDefault();

            return audioScript;
        }

        public async Task<CustomerVideoScript> CustomerVideoScript_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            CustomerVideoScript audioScript =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerVideoScript>(
                        Constants.StoredProcedures.AudioScriptTemplateSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .FirstOrDefault();

            return audioScript;
        }

        #endregion

        #region CustomerResource

        public async Task<CustomerResource> CustomerResource_SelectAsync(long customerResourceId)
        {
            CustomerResource resource =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerResource>(
                        Constants.StoredProcedures.CustomerResourceSelectById_sp,
                        dbParams: new {CustomerResourceId = customerResourceId}))
                    .FirstOrDefault();

            return resource;
        }

        public async Task<IEnumerable<CustomerResource>> CustomerResource_SelectActiveByCustomerIdAsync(long customerId)
        {
            List<CustomerResource> resources =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerResource>(
                        Constants.StoredProcedures.CustomerResourceSelectActiveByCustomerId_sp,
                        dbParams: new {CustomerId = customerId}))
                    .ToList();

            return resources;
        }

        public async Task<IEnumerable<CustomerResource>> CustomerResource_SelectByCustomerTemplateScriptIdAsync(
            long customerTemplateScriptId)
        {
            List<CustomerResource> resources =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerResource>(
                        Constants.StoredProcedures.CustomerResourceSelectByCustomerTemplateScriptId_sp,
                        dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId}))
                    .ToList();

            return resources;
        }

        public async Task<CustomerResource> CustomerResource_InsertAsync(CustomerResource customerResource)
        {
            customerResource.DateCreated = DateTime.UtcNow;
            CustomerResource result =
                ((IEnumerable<CustomerResource>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerResource>(
                            Constants.StoredProcedures.CustomerResourceInsert_sp,
                            dbParams: customerResource.ToPoco())))
                    .FirstOrDefault();

            return result;
        }

        public void CustomerResource_Update(CustomerResource customerResource)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerResourceUpdate_sp,
                dbParams: customerResource.ToPoco());
        }

        public void CustomerResource_Delete(long customerResourceId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.CustomerResourceDelete_sp,
                dbParams: new {CustomerResourceId = customerResourceId});
        }

        #endregion

        #region CustomerTemplateScript

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectAsync(long customerTemplateScriptId)
        {
            CustomerTemplateScript script =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(
                        Constants.StoredProcedures.CustomerTemplateScriptSelectById_sp,
                        dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId}))
                    .FirstOrDefault();

            return script;
        }

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(
            long customerTemplateScriptId)
        {
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                return await CustomerTemplateScript_SelectWithItemsAsync(conn, customerTemplateScriptId);
            }
        }

        public async Task<CustomerTemplateScript> CustomerTemplateScript_SelectWithItemsAsync(IDbConnection conn,
            long customerTemplateScriptId)
        {
            CustomerTemplateScript script =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(conn,
                        Constants.StoredProcedures.CustomerTemplateScriptSelectById_sp,
                        dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId}))
                    .FirstOrDefault();
            if (script == null)
            {
                return null;
            }

            Task<IEnumerable<TemplateScript>> templateTask =
                DataWrapper.QueryStoredProcAsync<TemplateScript>(Constants.StoredProcedures.TemplateScriptSelectById_sp,
                    dbParams: new {TemplateScriptId = script.fk_TemplateScriptId});
            Task<IEnumerable<TemplateScriptItem>> templateItemsTask =
                DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                    Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                    dbParams: new {TemplateScriptId = script.fk_TemplateScriptId});
            Task<IEnumerable<CustomerTemplateScriptItem>> itemsTask =
                DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(conn,
                    Constants.StoredProcedures.CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp,
                    dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId});
            Task<IEnumerable<CustomerResource>> resourcesTask = DataWrapper.QueryStoredProcAsync<CustomerResource>(conn,
                Constants.StoredProcedures.CustomerResourceSelectByCustomerTemplateScriptId_sp,
                dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId});

            await Task.WhenAll(templateTask, templateItemsTask, itemsTask, resourcesTask);

            TemplateScript template = templateTask.Result.FirstOrDefault();
            List<TemplateScriptItem> templateItems = templateItemsTask.Result.ToList();
            List<CustomerTemplateScriptItem> items = itemsTask.Result.ToList();
            List<CustomerResource> resources = resourcesTask.Result.ToList();

            // Map resource to item
            foreach (CustomerTemplateScriptItem item in items)
            {
                item.Resource = resources.FirstOrDefault(r => r.pk_CustomerResourceId == item.fk_CustomerResourceId);
                item.ScriptItem =
                    templateItems.FirstOrDefault(ti => ti.pk_TemplateScriptItemId == item.fk_TemplateScriptItemId);
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

        public async Task<CustomerTemplateScript> CustomerTemplateScript_InsertAsync(
            CustomerTemplateScript customerTemplateScript)
        {
            customerTemplateScript.DateCreated = DateTime.UtcNow;
            CustomerTemplateScript result =
                ((IEnumerable<CustomerTemplateScript>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerTemplateScript>(
                            Constants.StoredProcedures.CustomerTemplateScriptInsert_sp,
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

        public async Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_SelectByIdAsync(
            long customerTemplateScriptItemId)
        {
            CustomerTemplateScriptItem scriptItem =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(
                        Constants.StoredProcedures.CustomerTemplateScriptItemSelectById_sp,
                        dbParams: new {CustomerTemplateScriptItemId = customerTemplateScriptItemId}))
                    .FirstOrDefault();

            return scriptItem;
        }

        public async Task<IEnumerable<CustomerTemplateScriptItem>>
            CustomerTemplateScriptItem_SelectByCustomerTemplateScriptIdAsync(long customerTemplateScriptId)
        {
            List<CustomerTemplateScriptItem> scriptItems =
                (await
                    DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(
                        Constants.StoredProcedures.CustomerTemplateScriptItemSelectByCustomerTemplateScriptId_sp,
                        dbParams: new {CustomerTemplateScriptId = customerTemplateScriptId}))
                    .ToList();

            return scriptItems;
        }

        public async Task<CustomerTemplateScriptItem> CustomerTemplateScriptItem_InsertAsync(
            CustomerTemplateScriptItem customerTemplateScriptItem)
        {
            CustomerTemplateScriptItem result =
                ((IEnumerable<CustomerTemplateScriptItem>)
                    (await
                        DataWrapper.QueryStoredProcAsync<CustomerTemplateScriptItem>(
                            Constants.StoredProcedures.CustomerTemplateScriptItemInsert_sp,
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
            Organization organization =
                (await
                    DataWrapper.QueryStoredProcAsync<Organization>(Constants.StoredProcedures.OrganizationSelectById_sp,
                        dbParams: new {OrganizationId = organizationId}))
                    .FirstOrDefault();

            return organization;
        }

        #endregion

        #region StockAudio

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectAllAsync()
        {
            List<StockAudio> stockAudio =
                (await DataWrapper.QueryStoredProcAsync<StockAudio>(Constants.StoredProcedures.StockAudioSelectAll_sp))
                    .ToList();
            return stockAudio;
        }

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            List<StockAudio> stockAudio =
                (await
                    DataWrapper.QueryStoredProcAsync<StockAudio>(
                        Constants.StoredProcedures.StockAudioSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .ToList();
            return stockAudio;
        }

        public async Task<IEnumerable<StockAudio>> StockAudio_SelectByOrganizationIdAndVerticalIdAsync(
            int? organizationId, int? verticalId)
        {
            List<StockAudio> stockAudio =
                (await
                    DataWrapper.QueryStoredProcAsync<StockAudio>(
                        Constants.StoredProcedures.StockAudioSelectByOrganizationIdAndVerticalId_sp,
                        dbParams: new
                        {
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
            List<StockVideo> stockVideo =
                (await DataWrapper.QueryStoredProcAsync<StockVideo>(Constants.StoredProcedures.StockVideoSelectAll_sp))
                    .ToList();
            return stockVideo;
        }

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            List<StockVideo> stockVideo =
                (await
                    DataWrapper.QueryStoredProcAsync<StockVideo>(
                        Constants.StoredProcedures.StockVideoSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .ToList();
            return stockVideo;
        }

        public async Task<IEnumerable<StockVideo>> StockVideo_SelectByOrganizationIdAndVerticalIdAsync(
            int? organizationId, int? verticalId)
        {
            List<StockVideo> stockVideo =
                (await
                    DataWrapper.QueryStoredProcAsync<StockVideo>(
                        Constants.StoredProcedures.StockVideoSelectByOrganizationIdAndVerticalId_sp,
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
            List<StockVideo> stockVideo =
                (await
                    DataWrapper.QueryStoredProcAsync<StockVideo>(
                        Constants.StoredProcedures.StockVideoSelectByStoryboardId_sp,
                        dbParams: new {StoryboardId = storyboardId}))
                    .ToList();
            return stockVideo;
        }

        #endregion

        #region Storyboard

        public void Storyboard_Delete(int storyboardId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.StoryboardDelete_sp,
                dbParams: new {StoryboardId = storyboardId});
        }

        public async Task<Storyboard> Storyboard_InsertAsync(Storyboard storyboard)
        {
            storyboard.DateCreated = DateTime.UtcNow;
            Storyboard newStoryboard =
                ((IEnumerable<Storyboard>)
                    (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardInsert_sp,
                        dbParams: storyboard.ToPoco())))
                    .FirstOrDefault();
            return newStoryboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectAllAsync()
        {
            List<Storyboard> storyboard =
                (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectAll_sp))
                    .ToList();
            return storyboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            List<Storyboard> storyboard =
                (await
                    DataWrapper.QueryStoredProcAsync<Storyboard>(
                        Constants.StoredProcedures.StoryboardSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .ToList();
            return storyboard;
        }

        public async Task<Storyboard> Storyboard_SelectAsync(int storyboardId)
        {
            Storyboard storyboard =
                (await DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectById_sp,
                    dbParams: new {StoryboardId = storyboardId}))
                    .FirstOrDefault();
            return storyboard;
        }

        public async Task<Storyboard> Storyboard_SelectWithItemsAsync(int storyboardId)
        {
            Task<IEnumerable<Storyboard>> storyboardTask =
                DataWrapper.QueryStoredProcAsync<Storyboard>(Constants.StoredProcedures.StoryboardSelectById_sp,
                    dbParams: new {StoryboardId = storyboardId});
            Task<IEnumerable<StoryboardItem>> itemsTask =
                DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                    Constants.StoredProcedures.StoryboardItemSelectByStoryboardId_sp,
                    dbParams: new {StoryboardId = storyboardId});

            await Task.WhenAll(storyboardTask, itemsTask);

            Storyboard storyboard = storyboardTask.Result.FirstOrDefault();
            if (storyboard != null)
            {
                List<StoryboardItem> items = itemsTask.Result.ToList();
                storyboard.Items = items;
            }


            return storyboard;
        }

        public async Task<IEnumerable<Storyboard>> Storyboard_SelectByOrganizationIdAndVerticalIdAsync(
            int? organizationId, int? verticalId)
        {
            List<Storyboard> storyboard =
                (await
                    DataWrapper.QueryStoredProcAsync<Storyboard>(
                        Constants.StoredProcedures.StoryboardSelectByOrganizationIdAndVerticalId_sp,
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
            StoryboardItem result =
                ((IEnumerable<StoryboardItem>)
                    (await
                        DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                            Constants.StoredProcedures.StoryboardItemInsert_sp,
                            dbParams: storyboardItem.ToPoco())))
                    .FirstOrDefault();

            return result;
        }

        public void StoryboardItem_Delete(long storyboardItemId)
        {
            DataWrapper.ExecuteStoredProc(Constants.StoredProcedures.StoryboardItemDelete_sp,
                dbParams: new {StoryboardItemId = storyboardItemId});
        }

        public async Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByCustomerVideoIdAsync(long customerVideoId)
        {
            List<StoryboardItem> storyboardItems =
                (await
                    DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                        Constants.StoredProcedures.StoryboardItemSelectByCustomerVideoId_sp,
                        dbParams: new {CustomerVideoId = customerVideoId}))
                    .ToList();
            return storyboardItems;
        }

        public async Task<StoryboardItem> StoryboardItem_SelectByIdAsync(long storyboardItemId)
        {
            StoryboardItem storyboardItem =
                (await
                    DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                        Constants.StoredProcedures.StoryboardItemSelectById_sp,
                        dbParams: new {StoryboardItemId = storyboardItemId}))
                    .FirstOrDefault();
            return storyboardItem;
        }

        public async Task<IEnumerable<StoryboardItem>> StoryboardItem_SelectByStoryboardIdAsync(int storyboardId)
        {
            List<StoryboardItem> storyboardItems =
                (await
                    DataWrapper.QueryStoredProcAsync<StoryboardItem>(
                        Constants.StoredProcedures.StoryboardItemSelectByStoryboardId_sp,
                        dbParams: new {StoryboardId = storyboardId}))
                    .ToList();
            return storyboardItems;
        }

        #endregion

        #region TemplateScript

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectAllWithItemsAsync()
        {
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                Task<IEnumerable<TemplateScript>> scriptsTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectAll_sp);
                Task<IEnumerable<TemplateScriptItem>> itemsTask =
                    DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                        Constants.StoredProcedures.TemplateScriptItemSelectAll_sp);

                await Task.WhenAll(scriptsTask, itemsTask);

                List<TemplateScript> scripts = scriptsTask.Result.ToList();
                List<TemplateScriptItem> items = itemsTask.Result.ToList();

                foreach (TemplateScript script in scripts)
                {
                    script.Items.AddRange(
                        items.Where(i => i.fk_TemplateScriptId == script.pk_TemplateScriptId));
                }
                return scripts;
            }
        }

        public async Task<TemplateScript> TemplateScript_SelectAsync(long templateScriptId)
        {
            TemplateScript script =
                (await
                    DataWrapper.QueryStoredProcAsync<TemplateScript>(
                        Constants.StoredProcedures.TemplateScriptSelectById_sp,
                        dbParams: new {TemplateScriptId = templateScriptId}))
                    .FirstOrDefault();

            return script;
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByOrganizationIdAndVerticalIdAsync(
            int? organizationId, int? verticalId)
        {
            List<TemplateScript> templateScripts =
                (await
                    DataWrapper.QueryStoredProcAsync<TemplateScript>(
                        Constants.StoredProcedures.TemplateScriptSelectByOrganizationIdAndVerticalId_sp,
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
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                Task<IEnumerable<TemplateScript>> scriptTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectById_sp,
                    dbParams: new {TemplateScriptId = templateScriptId});
                Task<IEnumerable<TemplateScriptItem>> itemsTask =
                    DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                        Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                        dbParams: new {TemplateScriptId = templateScriptId});

                await Task.WhenAll(scriptTask, itemsTask);

                TemplateScript script = scriptTask.Result.FirstOrDefault();

                if (script != null)
                {
                    List<TemplateScriptItem> items = itemsTask.Result.ToList();
                    script.Items.AddRange(items);
                }
                return script;
            }
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdAsync(int storyboardId)
        {
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                List<TemplateScript> templateScripts = (await DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectByStoryboardId_sp,
                    dbParams: new {StoryboardId = storyboardId})
                    ).ToList();

                return templateScripts;
            }
        }

        public async Task<IEnumerable<TemplateScript>> TemplateScript_SelectByStoryboardIdWithItemsAsync(
            int storyboardId)
        {
            using (IDbConnection conn = DataWrapper.OpenConnection())
            {
                Task<IEnumerable<TemplateScript>> scriptsTask = DataWrapper.QueryStoredProcAsync<TemplateScript>(conn,
                    Constants.StoredProcedures.TemplateScriptSelectByStoryboardId_sp,
                    dbParams: new {StoryboardId = storyboardId});
                Task<IEnumerable<TemplateScriptItem>> itemsTask =
                    DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(conn,
                        Constants.StoredProcedures.TemplateScriptItemSelectByStoryboardId_sp,
                        dbParams: new {StoryboardId = storyboardId});

                await Task.WhenAll(scriptsTask, itemsTask);

                List<TemplateScript> scripts = scriptsTask.Result.ToList();
                List<TemplateScriptItem> items = itemsTask.Result.ToList();

                foreach (TemplateScript script in scripts)
                {
                    List<TemplateScriptItem> matchingItems =
                        items.Where(tsi => tsi.fk_TemplateScriptId == script.pk_TemplateScriptId)
                            .ToList();
                    script.Items.AddRange(matchingItems);
                }
                return scripts;
            }
        }

        #endregion

        #region TemplateScriptItem

        public async Task<IEnumerable<TemplateScriptItem>> TemplateScriptItem_SelectByTemplateScriptIdAsync(
            long templateScriptId)
        {
            List<TemplateScriptItem> items =
                (await
                    DataWrapper.QueryStoredProcAsync<TemplateScriptItem>(
                        Constants.StoredProcedures.TemplateScriptItemSelectByTemplateScriptId_sp,
                        dbParams: new {TemplateScriptId = templateScriptId}))
                    .ToList();
            return items;
        }

        #endregion

        #region VoiceActor

        public async Task<IEnumerable<VoiceActor>> VoiceActor_SelectAllAsync()
        {
            List<VoiceActor> voiceActors =
                (await DataWrapper.QueryStoredProcAsync<VoiceActor>(Constants.StoredProcedures.VoiceActorSelectAll_sp))
                    .ToList();
            return voiceActors;
        }

        #endregion
    }
}