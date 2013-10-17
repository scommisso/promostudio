using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using log4net;
using Newtonsoft.Json;
using PromoStudio.Common.Encryption;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Rendering.Properties;
using PromoStudio.Web.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class BuildController : ControllerBase
    {
        private ICryptoManager _cryptoManager;

        public BuildController(IDataService dataService, ILog log, ICryptoManager cryptoManager)
            : base(dataService, log)
        {
            _cryptoManager = cryptoManager;
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
                || data.Video == null
                || data.Video.fk_CustomerId != customerId)
            {
                ClearCookie();
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
                    ClearCookie();
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

            storyboards = storyboards
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards)
                .Concat(storyboards);

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

            var storyboardTask = _dataService.Storyboard_SelectWithItemsAsync(data.Video.fk_StoryboardId);
            var templatesTask = _dataService.TemplateScript_SelectByStoryboardIdWithItemsAsync(data.Video.fk_StoryboardId);

            await Task.WhenAll(storyboardTask, templatesTask);

            var storyboard = storyboardTask.Result;
            var templates = templatesTask.Result.ToList();
            foreach (var sbi in storyboard.Items)
            {
                if (!sbi.fk_TemplateScriptId.HasValue) { continue; }
                sbi.TemplateScript = templates.FirstOrDefault(ts => ts.pk_TemplateScriptId == sbi.fk_TemplateScriptId);
            }

            data.Video.Storyboard = storyboard;
            var vm = new BrandingViewModel()
            {
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
                CreateCookie(cookieModel);
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

        private const string buildCookieKey = "BuildData";
        private BuildCookieViewModel _buildCookieViewModel = null;
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

        private BuildCookieViewModel GetFromCookie()
        {
            if (_buildCookieViewModel != null)
            {
                return _buildCookieViewModel;
            }

            var cookie = Request.Cookies.Get(buildCookieKey);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return null;
            }

            try
            {
                var json = _cryptoManager.DecryptString(
                    cookie.Value, Properties.Settings.Default.BuildCookieSecret);
                var video = JsonConvert.DeserializeObject<BuildCookieViewModel>(json);
                _buildCookieViewModel = video;
                return video;
            }
            catch
            {
                // invalid data, expire the bad cookie
                _buildCookieViewModel = null;
                ClearCookie();
                return null;
            }
        }

        private void ClearCookie()
        {
            var cookie = new HttpCookie(buildCookieKey, string.Empty);
            cookie.Expires = DateTime.Today.AddYears(-5);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }

        private void CreateCookie(BuildCookieViewModel video)
        {
            var json = JsonConvert.SerializeObject(video);
            var encrypted = _cryptoManager.EncryptString(json, Properties.Settings.Default.BuildCookieSecret);
            var cookie = new HttpCookie(buildCookieKey, encrypted);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.HttpOnly = true;

            Response.Cookies.Add(cookie);
        }
    }
}
