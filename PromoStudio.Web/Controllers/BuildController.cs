using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using log4net;
using Newtonsoft.Json;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class BuildController : ControllerBase
    {
        public BuildController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Build/
        public async Task<ActionResult> Index()
        {
            var items = (await _dataService.TemplateScript_SelectAllWithItemsAsync());
            var videos = (await _dataService.StockVideo_SelectAll());
            var audio = (await _dataService.StockAudio_SelectAll());
            var resources = (await _dataService.CustomerResource_SelectActiveByCustomerIdAsync(1));

            ViewBag.CustomerResourcesJson = JsonConvert.SerializeObject(resources);
            ViewBag.TemplateScriptsJson = JsonConvert.SerializeObject(items);
            ViewBag.StockVideosJson = JsonConvert.SerializeObject(videos);
            ViewBag.StockAudioJson = JsonConvert.SerializeObject(audio);

            return PAjax();
        }

        //
        // GET: /Build/Media-Logo
        [ActionName("Media-Logo")]
        public ActionResult MediaLogo()
        {
            return PAjax("_Media-Logo");
        }

        //
        // GET: /Build/Script
        public ActionResult Script()
        {
            return PAjax("_Script");
        }

        //
        // GET: /Build/Footage
        public ActionResult Footage()
        {
            return PAjax("_Footage");
        }

        //
        // GET: /Build/Status?cvid={cvid}
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
        // POST: /Build/
        [HttpPost]
        public async Task<ActionResult> Submit(CustomerVideo video)
        {
            if (HttpContext.User == null || HttpContext.User.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var customer = HttpContext.User.Identity as PromoStudioIdentity;
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            string json = JsonConvert.SerializeObject(video);
            long customerId = customer.CustomerId;
            _log.InfoFormat("Customer {0} submitted video: {1}", customerId, json);
            try
            {
                video.DateCreated = DateTime.Now;
                video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.Canceled; // store as canceled state until all children have populated
                var newVideo = await _dataService.CustomerVideo_InsertAsync(video);
                foreach (var item in video.Items)
                {
                    item.fk_CustomerVideoId = newVideo.pk_CustomerVideoId;
                    if (item.fk_CustomerVideoItemTypeId == (sbyte)CustomerVideoItemType.CustomerTemplateScript)
                    {
                        item.CustomerScript.DateCreated = DateTime.Now;
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

                newVideo = await _dataService.CustomerVideo_SelectAsyncWithItems(newVideo.pk_CustomerVideoId);
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

    }
}
