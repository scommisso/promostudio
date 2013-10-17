using log4net;
using PromoStudio.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return PAjax();
        }

        //
        // GET: /Videos/Data
        public async Task<ActionResult> Data()
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

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
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

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
