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
    public class VideosController : ControllerBase
    {
        public VideosController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Videos/
        public async Task<ActionResult> Index()
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

            return PAjax();
        }

        //
        // GET: /Videos/Data
        public async Task<ActionResult> Data()
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
            long customerId = customer.CustomerId;

            var customerInfo = (await _dataService.Customer_SelectAsync(customerId));
            if (customerInfo == null)
            {
                return new HttpNotFoundResult();
            }

            var videos = (await _dataService.CustomerVideo_SelectByCustomerIdAsync(customerId));

            return Json(new
            {
                Customer = customerInfo,
                CustomerVideos = videos
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Videos/Status
        public async Task<ActionResult> Status()
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
            long customerId = customer.CustomerId;

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
