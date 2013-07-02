using System;
using System.Collections.Generic;
using System.IO;
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
using PromoStudio.Rendering.Properties;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        public ResourcesController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Resources/
        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return PAjax();
        }

        //
        // GET: /Resources/Data
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

            var resources = (await _dataService.CustomerResource_SelectActiveByCustomerIdAsync(customerId))
                .Where(r => r.Type != TemplateScriptItemType.Text)
                .ToList();
            
            return Json(new
            {
                Customer = customerInfo,
                CustomerResources = resources
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Resources/Upload?category={category}
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, int category)
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

            if (file.ContentLength > 0)
            {
                // TODO: Check for allowed file extensions and infer type
                TemplateScriptItemType type = TemplateScriptItemType.Image;

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Settings.Default.UploadPath, string.Format("{0}\\{1}", customerId, fileName));
                file.SaveAs(path);

                var res = new CustomerResource() {
                    fk_CustomerId = customerId,
                    fk_CustomerResourceStatusId = (sbyte) CustomerResourceStatus.Active,
                    fk_TemplateScriptItemCategoryId = (sbyte) category,
                    fk_TemplateScriptItemTypeId = (sbyte) type,
                    Value = path                    
                };
                res = await _dataService.CustomerResource_InsertAsync(res);

                // TODO: Log upload
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Resources/Download?crid={crid}
        public async Task<ActionResult> Download(long crid)
        {
            // TODO: Use login cookie
            long customerId = 1;

            var customer = (await _dataService.Customer_SelectAsync(customerId));
            if (customer == null)
            {
                return new HttpNotFoundResult();
            }

            var resource = (await _dataService.CustomerResource_Select(crid));
            if (resource.fk_CustomerId != customerId)
            {
                return new HttpNotFoundResult();
            }

            var type = (TemplateScriptItemType)resource.fk_TemplateScriptItemTypeId;
            string contentType;
            switch (type)
            {
                case TemplateScriptItemType.Audio:
                    contentType = "audio/mpeg3";
                    break;
                case TemplateScriptItemType.Image:
                    contentType = "image/png";
                    break;
                case TemplateScriptItemType.Video:
                    contentType = "video/mp4";
                    break;
                case TemplateScriptItemType.Text:
                default:
                    contentType = "text/plain";
                    break;
            }

            if (string.IsNullOrEmpty(resource.Value))
            {
                return new HttpNotFoundResult();
            }
            if (type != TemplateScriptItemType.Text && !System.IO.File.Exists(resource.Value))
            {
                return new HttpNotFoundResult();
            }

            return File(resource.Value, contentType);
        }
    }
}
