using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using log4net;
using Newtonsoft.Json;
using PromoStudio.Common.Encryption;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Serialization;
using PromoStudio.Data;
using PromoStudio.Rendering.Properties;
using PromoStudio.Web.Models.Session;
using PromoStudio.Web.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CustomerVideo = PromoStudio.Common.Models.CustomerVideo;
using CustomerVideoItem = PromoStudio.Common.Models.CustomerVideoItem;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class BuildController : ControllerBase
    {
        private ICryptoManager _cryptoManager;
        private ISerializationManager _serializationManager;

        public BuildController(IDataService dataService, ILog log, ICryptoManager cryptoManager, ISerializationManager serializationManager)
            : base(dataService, log)
        {
            _cryptoManager = cryptoManager;
            _serializationManager = serializationManager;
        }

        //
        // GET: /Build/
        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

            var data = GetFromCookie();
            if (data == null
                || data.CompletedSteps == null
                || data.CompletedSteps.Count == 0
                || data.Video == null)
            {
                ClearBuildCookies();
                return new RedirectResult("~/Build/Footage");
            }

            int lastComplete = 0;
            for (int i = 0; i < data.CompletedSteps.Count; i++)
            {
                if ((data.CompletedSteps[i] - lastComplete) > 1)
                {
                    break;
                }
                lastComplete = data.CompletedSteps[i];
            }

            switch (lastComplete)
            {
                case 1:
                    return new RedirectResult("~/Build/Branding");
                case 2:
                    return new RedirectResult("~/Build/Script");
                case 3:
                    return new RedirectResult("~/Build/Audio");
                case 4:
                    return new RedirectResult("~/Build/Summary");
                case 0:
                case 5:
                default:
                    ClearBuildCookies();
                    return new RedirectResult("~/Build/Footage");
            }
        }

        //
        // GET: /Build/Footage
        [ActionName("Footage")]
        public async Task<ActionResult> Footage()
        {
            if (!ValidateUserStep(1))
            {
                return new RedirectResult("~/Build");
            }

            var storyboards = (await _dataService.Storyboard_SelectByOrganizationIdAndVerticalIdAsync(
                CurrentUser.OrganizationId, CurrentUser.VerticalId));

            var data = GetFromCookie();
            var vm = new FootageViewModel()
            {
                StepsCompleted = (data == null || data.CompletedSteps == null)
                    ? new List<int>()
                    : new List<int>(data.CompletedSteps),
                Storyboards = storyboards.ToList(),
                Video = (data == null || data.Video == null)
                    ? new CustomerVideo()
                        {
                            pk_CustomerVideoId = -1,
                            fk_CustomerId = CurrentUser.CustomerId,
                            DateCreated = DateTime.Now
                        }
                    : data.Video
            };

            return PAjax("Footage", model: vm);
        }

        //
        // GET: /Build/Branding
        [ActionName("Branding")]
        public async Task<ActionResult> Branding()
        {
            if (!ValidateUserStep(2))
            {
                return new RedirectResult("~/Build");
            }

            var data = GetFromCookie();

            // Get required db items
            var tasks = new List<Task>();
            Task<Common.Models.Organization> orgTask = null;
            var storyboardTask = _dataService.Storyboard_SelectWithItemsAsync(data.Video.fk_StoryboardId);
            var templatesTask =
                _dataService.TemplateScript_SelectByStoryboardIdWithItemsAsync(data.Video.fk_StoryboardId);
            var resourcesTask = _dataService.CustomerResource_SelectActiveByCustomerIdAsync(CurrentUser.CustomerId);
            tasks.AddRange(new Task[] {storyboardTask, templatesTask, resourcesTask});

            if (CurrentUser.OrganizationId.HasValue)
            {
                orgTask = _dataService.Organization_SelectAsync(CurrentUser.OrganizationId.Value);
            }

            await Task.WhenAll(tasks);

            Common.Models.Organization organization = null;
            var storyboard = storyboardTask.Result;
            var templates = templatesTask.Result.ToList();
            var resources = resourcesTask.Result.ToList();
            if (orgTask != null)
            {
                organization = orgTask.Result;
            }
            foreach (var sbi in storyboard.Items)
            {
                if (sbi.fk_TemplateScriptId.HasValue)
                {
                    sbi.TemplateScript =
                        templates.FirstOrDefault(ts => ts.pk_TemplateScriptId == sbi.fk_TemplateScriptId);
                }
            }

            data.Video.Storyboard = storyboard;

            // Link items to storyboard if populated
            foreach (var cvItem in data.Video.Items)
            {
                if (cvItem.CustomerScript != null)
                {
                    foreach (var csi in cvItem.CustomerScript.Items)
                    {
                        csi.ScriptItem = templates
                            .SelectMany(t => t.Items)
                            .FirstOrDefault(ti => ti.pk_TemplateScriptItemId == csi.fk_TemplateScriptItemId);
                        if (csi.fk_CustomerResourceId > 0)
                        {
                            csi.Resource =
                                resources.FirstOrDefault(cr => cr.pk_CustomerResourceId == csi.fk_CustomerResourceId);
                        }
                    }
                }
            }

            var vm = new BrandingViewModel()
            {
                Organization = organization,
                Video = data.Video,
                StepsCompleted = new List<int>(data.CompletedSteps)
            };

            return PAjax("Branding", model: vm);
        }

        //
        // GET: /Build/Script
        public async Task<ActionResult> Script()
        {
            if (!ValidateUserStep(3))
            {
                return new RedirectResult("~/Build");
            }


            var data = GetFromCookie();
            var vm = new ScriptViewModel()
            {
                Video = data.Video,
                StepsCompleted = new List<int>(data.CompletedSteps)
            };

            return PAjax("Script", model: vm);
        }

        //
        // GET: /Build/Audio
        [ActionName("Audio")]
        public async Task<ActionResult> Audio()
        {
            if (!ValidateUserStep(4))
            {
                return new RedirectResult("~/Build");
            }



            var data = GetFromCookie();
            var vm = new AudioViewModel()
            {
                Video = data.Video,
                StepsCompleted = new List<int>(data.CompletedSteps)
            };

            return PAjax("Audio", model: vm);
        }

        //
        // GET: /Build/Summary
        [ActionName("Summary")]
        public async Task<ActionResult> Summary()
        {
            if (!ValidateUserStep(5))
            {
                return new RedirectResult("~/Build");
            }

            var data = GetFromCookie();
            var vm = new SummaryViewModel()
            {
                Video = data.Video,
                StepsCompleted = new List<int>(data.CompletedSteps)
            };

            return PAjax("Summary", model: vm);
        }

        //
        // GET: /Build/Status?cvid={cvid}
        [OutputCache(NoStore = true)]
        public async Task<ActionResult> Status(long cvid)
        {
            try
            {
                var video = (await _dataService.CustomerVideo_SelectAsync(cvid));
                if (video == null)
                {
                    return new HttpNotFoundResult();
                }

                return Json(video, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error("Error retrieving status for customer video id: " + cvid, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //
        // POST: /Build/Checkpoint
        [HttpPost]
        public ActionResult Checkpoint(BuildCookieViewModel cookieModel)
        {
            if (CurrentUser == null
                || cookieModel == null
                || cookieModel.Video == null
                || cookieModel.Video.fk_CustomerId <= 0
                || cookieModel.Video.fk_CustomerId != CurrentUser.CustomerId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            try
            {
                MergeData(cookieModel);
                CreateCookies(cookieModel);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Error creating video cookie for customer id: {0}", CurrentUser.CustomerId), ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //
        // POST: /Build/Submit
        [HttpPost]
        public async Task<ActionResult> Submit(CustomerVideo video)
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

            string json = JsonConvert.SerializeObject(video);
            _log.InfoFormat("Customer {0} submitted video: {1}", customerId, json);
            try
            {
                video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.Canceled; // store as canceled state until all children have populated
                var newVideo = await _dataService.CustomerVideo_InsertAsync(video);
                foreach (var item in video.Items)
                {
                    item.fk_CustomerVideoId = newVideo.pk_CustomerVideoId;
                    if (item.fk_CustomerVideoItemTypeId == (sbyte)CustomerVideoItemType.CustomerTemplateScript)
                    {
                        item.CustomerScript.fk_CustomerId = customerId;
                        var newScript = await _dataService.CustomerTemplateScript_InsertAsync(item.CustomerScript);
                        foreach (var scriptItem in item.CustomerScript.Items)
                        {
                            scriptItem.fk_CustomerTemplateScriptId = newScript.pk_CustomerTemplateScriptId;
                            if (scriptItem.fk_CustomerResourceId <= 0 && scriptItem.Resource != null)
                            {
                                scriptItem.Resource.fk_CustomerId = customerId;
                                var newResource = await _dataService.CustomerResource_InsertAsync(scriptItem.Resource);
                                scriptItem.fk_CustomerResourceId = newResource.pk_CustomerResourceId;
                            }
                            await _dataService.CustomerTemplateScriptItem_InsertAsync(scriptItem);
                        }
                        item.fk_CustomerVideoItemId = newScript.pk_CustomerTemplateScriptId;
                    }
                    await _dataService.CustomerVideoItem_InsertAsync(item);
                }

                newVideo = await _dataService.CustomerVideo_SelectWithItemsAsync(newVideo.pk_CustomerVideoId);
                newVideo.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.Pending;
                _dataService.CustomerVideo_Update(newVideo);

                return Json(new { Success = true, Model = newVideo });
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Error submitting video for customer id: {0}, JSON: {1}", customerId, json), ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        private bool ValidateUserStep(int stepId)
        {
            if (stepId == 1)
            {
                // let anyone into step 1
                return true;
            }

            var data = GetFromCookie();
            if (data == null
                || data.CompletedSteps == null
                || data.CompletedSteps.Count == 0
                || data.Video == null
                || data.Video.fk_CustomerId != CurrentUser.CustomerId)
            {
                return false;
            }

            return data.CompletedSteps.Contains(stepId - 1);
        }

        private void MergeData(BuildCookieViewModel newCookie)
        {
            var oldCookie = GetFromCookie();
            if (oldCookie == null)
            {
                return;
            }

            var oldCookieVideo = oldCookie.Video;
            var newCookieVideo = newCookie.Video;

            if (oldCookieVideo.fk_StoryboardId != newCookieVideo.fk_StoryboardId)
            {
                // user changed the storyboard
                newCookieVideo.Items.Clear();
            }
            else
            {
                // ensure that video has the matching footage items
                foreach (var cvItem in newCookieVideo.Items)
                {
                    if (cvItem.StockAudio != null
                        || cvItem.StockVideo != null
                        || cvItem.CustomerScript != null
                        || cvItem.VoiceOver != null)
                    {
                        continue; // data already present
                    }
                    var matchItem = oldCookieVideo.Items
                        .FirstOrDefault(ocvItem =>
                            ocvItem.fk_CustomerVideoItemTypeId == cvItem.fk_CustomerVideoItemTypeId
                            && ocvItem.SortOrder == cvItem.SortOrder);
                    if (matchItem != null)
                    {
                        cvItem.StockAudio = matchItem.StockAudio;
                        cvItem.StockVideo = matchItem.StockVideo;
                        cvItem.CustomerScript = matchItem.CustomerScript;
                        cvItem.VoiceOver = matchItem.VoiceOver;
                    }
                }
            }
        }

        private const string buildCookieStateKey = "BuildData_State";
        private const string buildCookieVideoItemsKey = "BuildData_VideoItems";
        private const string buildCookieTemplateKey = "BuildData_Template";
        private const string buildCookieTemplateItemsKey = "BuildData_TemplateItems";
        private const string buildCookieResourcesKey = "BuildData_Resources";
        private BuildCookieViewModel _buildCookieViewModel = null;
        private BuildCookieViewModel GetFromCookie()
        {
            if (_buildCookieViewModel != null)
            {
                return _buildCookieViewModel;
            }

            var buildState = GetBuildCookieValue<BuildState>(buildCookieStateKey);
            if (buildState == null || buildState.Video.fk_CustomerId != CurrentUser.CustomerId)
            {
                ClearBuildCookies();
                return null;
            }

            // get the cookies
            var videoItemsState = GetBuildCookieValue<Models.Session.CustomerVideoItem[]>(buildCookieVideoItemsKey);
            var templatesState = GetBuildCookieValue<Models.Session.CustomerTemplateScript[]>(buildCookieTemplateKey);
            var templateItemsState = GetBuildCookieValue<Models.Session.CustomerTemplateScriptItem[]>(buildCookieTemplateItemsKey);
            var resourcesState = GetBuildCookieValue<Models.Session.CustomerResource[]>(buildCookieResourcesKey);

            // combine the cookies back together
            _buildCookieViewModel = buildState.ToModel();
            var videoItems = videoItemsState.Select(vis => vis.ToModel()).ToList();
            var templates = templatesState.Select(ts => ts.ToModel()).ToList();
            var templateItems = templateItemsState.Select(tsi => tsi.ToModel()).ToList();
            var resources = resourcesState.Select(tsi => tsi.ToModel()).ToList();
            _buildCookieViewModel.Video.Items.AddRange(videoItems);
            foreach (var vidItem in videoItems.Where(vi => vi.Type == CustomerVideoItemType.CustomerTemplateScript))
            {
                vidItem.CustomerScript =
                    templates.FirstOrDefault(t => t.pk_CustomerTemplateScriptId == vidItem.fk_CustomerVideoItemId);
            }
            foreach (var template in templates)
            {
                template.Items.AddRange(templateItems.Where(ti => ti.fk_CustomerTemplateScriptId == template.pk_CustomerTemplateScriptId));
            }
            foreach (var templateItem in templateItems)
            {
                templateItem.Resource = resources.FirstOrDefault(r => r.pk_CustomerResourceId == templateItem.fk_CustomerResourceId);
            }

            return _buildCookieViewModel;
        }

        private T GetBuildCookieValue<T>(string key) where T : class
        {
            var cookie = Request.Cookies.Get(key);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return null;
            }

            try
            {
                var val = _serializationManager.DeserializeFromString<T>(cookie.Value);
                return val;
            }
            catch
            {
                // bad data, expire it
                ClearCookie(key);
                return null;
            }
        }

        private void ClearBuildCookies()
        {
            ClearCookie(buildCookieStateKey);
            ClearCookie(buildCookieVideoItemsKey);
            ClearCookie(buildCookieTemplateKey);
            ClearCookie(buildCookieTemplateItemsKey);
            ClearCookie(buildCookieResourcesKey);
        }

        private void ClearCookie(string key)
        {
            var cookie = new HttpCookie(key, string.Empty);
            cookie.Expires = DateTime.Today.AddYears(-5);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }

        private void CreateCookies(BuildCookieViewModel cookieModel)
        {
            var videoItems = cookieModel.Video.Items.ToList();
            var templates = videoItems.Where(vi => vi.CustomerScript != null).Select(vi => vi.CustomerScript).ToList();

            // Ensure all customer templates have IDs
            if (templates.Count > 0)
            {
                long minTemplateId = templates.Min(t => t.pk_CustomerTemplateScriptId);
                foreach (var template in templates)
                {
                    if (template.pk_CustomerTemplateScriptId == 0)
                    {
                        minTemplateId -= 1;
                        template.pk_CustomerTemplateScriptId = minTemplateId;
                        var matchingItem = videoItems.FirstOrDefault(cvi => cvi.CustomerScript == template);
                        if (matchingItem != null)
                        {
                            matchingItem.fk_CustomerVideoItemId = minTemplateId;
                        }
                    }
                    template.Items.ForEach(ti => ti.fk_CustomerTemplateScriptId = template.pk_CustomerTemplateScriptId);
                }
            }
            var templateItems = templates.SelectMany(t => t.Items).ToList();

            // Ensure all resources have IDs
            var resources = new List<Common.Models.CustomerResource>();
            if (templateItems.Count > 0)
            {
                long minResourceId = templates.Min(t => t.pk_CustomerTemplateScriptId);
                resources = templateItems.Where(ti => ti.Resource != null).Select(ti => ti.Resource).ToList();
                foreach (var resource in resources)
                {
                    if (resource.pk_CustomerResourceId == 0)
                    {
                        minResourceId -= 1;
                        resource.pk_CustomerResourceId = minResourceId;
                        var matchingItem = templateItems.FirstOrDefault(ti => ti.Resource == resource);
                        if (matchingItem != null)
                        {
                            matchingItem.fk_CustomerResourceId = minResourceId;
                        }
                    }
                }
            }
            
            // Create trimmed-down models for cookie storage
            var buildState = new BuildState(cookieModel);
            var videoItemsState = videoItems.Select(vi => new Models.Session.CustomerVideoItem(vi)).ToArray();
            var templatesState = templates.Select(t => new Models.Session.CustomerTemplateScript(t)).ToArray();
            var templateItemsState = templateItems.Select(ti => new Models.Session.CustomerTemplateScriptItem(ti)).ToArray();
            var resourcesState = resources.Select(r => new Models.Session.CustomerResource(r)).ToArray();

            var serializedBuildState = _serializationManager.SerializeToString(buildState);
            var serializedVideoItems = _serializationManager.SerializeToString(videoItemsState);
            var serializedTemplates = _serializationManager.SerializeToString(templatesState);
            var serializedTemplateItems = _serializationManager.SerializeToString(templateItemsState);
            var serializedResources = _serializationManager.SerializeToString(resourcesState);

            CreateBuildCookie(buildCookieStateKey, serializedBuildState);
            CreateBuildCookie(buildCookieVideoItemsKey, serializedVideoItems);
            CreateBuildCookie(buildCookieTemplateKey, serializedTemplates);
            CreateBuildCookie(buildCookieTemplateItemsKey, serializedTemplateItems);
            CreateBuildCookie(buildCookieResourcesKey, serializedResources);
        }

        private void CreateBuildCookie(string key, string value)
        {
            var cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
    }
}
