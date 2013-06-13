using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;

namespace PromoStudio.Web.Controllers
{
    public class VideosController : AsyncController
    {
        private IDataService _dataService;
        private long customerId = 1;

        public VideosController(IDataService dataService)
        {
            _dataService = dataService;
        }

        //
        // GET: /Videos/
        public async Task<ActionResult> Index()
        {
            // TODO: Use login cookie

            var customer = (await _dataService.Customer_SelectAsync(customerId));
            if (customer == null)
            {
                return new HttpNotFoundResult();
            }

            var videos = (await _dataService.CustomerVideo_SelectByCustomerIdAsync(customerId));

            ViewBag.Customer = customer;
            ViewBag.CustomerVideos = videos;
            ViewBag.CustomerJson = JsonConvert.SerializeObject(customer);
            ViewBag.CustomerVideosJson = JsonConvert.SerializeObject(videos);

            return View();
        }

        //
        // GET: /Videos/Status
        public async Task<ActionResult> Status()
        {
            // TODO: Use login cookie

            var customer = (await _dataService.Customer_SelectAsync(customerId));
            if (customer == null)
            {
                return new HttpNotFoundResult();
            }

            var videos = (await _dataService.CustomerVideo_SelectByCustomerIdAsync(customerId));

            return Json(videos, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Videos/Play?cvid={cvid}
        public async Task<ActionResult> Play(long cvid)
        {
            // TODO: Add filter here to only allow vidyard to pull down content - 404 for all other callers
            var video = (await _dataService.CustomerVideo_SelectAsync(cvid));
            if (video == null)
            {
                return new HttpNotFoundResult();
            }

            string path = null;
            if (!string.IsNullOrEmpty(video.PreviewFilePath)) {
                path = video.PreviewFilePath;
            }
            if (!string.IsNullOrEmpty(video.CompletedFilePath)) {
                path = video.CompletedFilePath;
            }
            if (string.IsNullOrEmpty(path))
            {
                return new HttpNotFoundResult();
            }

            return Redirect(path);
        }
    }
}
