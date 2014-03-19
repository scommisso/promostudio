using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using log4net;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Storage;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private IStorageProvider _storageProvider;

        public ResourcesController(IDataService dataService, ILog log, IStorageProvider storageProvider)
            : base(dataService, log)
        {
            _storageProvider = storageProvider;
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
        // GET: /Resources/Data?typeId={typeId}
        public async Task<ActionResult> Data(int? typeId)
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

            Customer customerInfo = (await _dataService.Customer_SelectAsync(customerId));
            if (customerInfo == null)
            {
                return new HttpNotFoundResult();
            }

            List<CustomerResource> resources =
                (await _dataService.CustomerResource_SelectActiveByCustomerIdAsync(customerId))
                    .Where(r => typeId.HasValue
                        ? r.fk_TemplateScriptItemTypeId == (sbyte) typeId.Value
                        : r.Type != TemplateScriptItemType.Text)
                    .ToList();

            return Json(new
            {
                Customer = customerInfo,
                CustomerResources = resources
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Resources/Upload?category={category}&isOrgResource={isOrgResource}&fileName={fileName}
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, int category, bool? isOrgResource, string fileName)
        {
            if (CurrentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            long customerId = CurrentUser.CustomerId;

            // Support both XHR and Forms uploads
            string filePath = null;
            Stream inputStream = null;
            if (file == null && Request.Files.Count > 0)
            {
                file = Request.Files[0];
            }
            if (file == null && Request.ContentLength > 0)
            {
                filePath = fileName;
                inputStream = Request.InputStream;
            }
            else if (file != null && file.ContentLength > 0)
            {
                filePath = Path.GetFileName(file.FileName);
                inputStream = file.InputStream;
            }
            if (inputStream == null || filePath == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                // Setup resource options
                int orgId = 0;
                string bucketName;
                if (isOrgResource == true && CurrentUser.OrganizationId.HasValue)
                {
                    orgId = CurrentUser.OrganizationId.Value;
                    bucketName = string.Format("org_{0}", orgId);
                }
                else
                {
                    bucketName = customerId.ToString();
                    isOrgResource = false;
                }

                // create file record
                // TODO: Check for allowed file extensions and infer type
                var type = TemplateScriptItemType.Image;
                var res = new CustomerResource
                {
                    fk_CustomerId = customerId,
                    fk_CustomerResourceStatusId = (sbyte) CustomerResourceStatus.Active,
                    fk_TemplateScriptItemCategoryId = (sbyte) category,
                    fk_TemplateScriptItemTypeId = (sbyte) type,
                    OriginalFileName = filePath,
                    Value = ""
                };
                if (isOrgResource == true)
                {
                    res.fk_CustomerId = null;
                    res.fk_OrganizationId = orgId;
                }

                res = await _dataService.CustomerResource_InsertAsync(res);

                // store file
                string fileId = res.pk_CustomerResourceId.ToString();
                string fileUrl = await _storageProvider.StoreFile(bucketName, fileId, inputStream);

                // TODO: Create thumbnail (228x130) and push to storage provider
                string thumbnailUrl = fileUrl;

                // Update resource pointer
                res.Value = fileUrl;
                res.ThumbnailUrl = thumbnailUrl;
                _dataService.CustomerResource_Update(res);

                // TODO: Log upload

                return new JsonResult()
                {
                    Data = res
                };
            }
            finally
            {
                inputStream.Dispose();
            }
        }

        //
        // GET: /Resources/ActorPhoto?voiceActorId={voiceActorId}
        [OutputCache(Duration = 604800)] // 1 week
        public async Task<ActionResult> ActorPhoto(int? voiceActorId)
        {
            if (!voiceActorId.HasValue)
            {
                return new HttpNotFoundResult();
            }
            VoiceActor actor = (await _dataService.VoiceActor_SelectAllAsync())
                .FirstOrDefault(va => va.pk_VoiceActorId == voiceActorId.Value);
            if (actor == null)
            {
                return new HttpNotFoundResult();
            }

            return File(actor.PhotoFilePath, MimeMapping.GetMimeMapping(actor.PhotoFilePath));
        }

        //
        // GET: /Resources/ActorSample?voiceActorId={voiceActorId}
        [OutputCache(Duration = 604800)] // 1 week
        public async Task<ActionResult> ActorSample(int? voiceActorId)
        {
            if (!voiceActorId.HasValue)
            {
                return new HttpNotFoundResult();
            }
            VoiceActor actor = (await _dataService.VoiceActor_SelectAllAsync())
                .FirstOrDefault(va => va.pk_VoiceActorId == voiceActorId.Value);
            if (actor == null)
            {
                return new HttpNotFoundResult();
            }

            return File(actor.SampleFilePath, MimeMapping.GetMimeMapping(actor.SampleFilePath));
        }

        //
        // GET: /Resources/StockAudio?stockAudioId={stockAudioId}
        [OutputCache(Duration = 604800)] // 1 week
        public async Task<ActionResult> StockAudio(int? stockAudioId)
        {
            if (!stockAudioId.HasValue)
            {
                return new HttpNotFoundResult();
            }
            StockAudio audio = (await _dataService.StockAudio_SelectAllAsync())
                .FirstOrDefault(sa => sa.pk_StockAudioId == stockAudioId.Value);
            if (audio == null)
            {
                return new HttpNotFoundResult();
            }

            return File(audio.FilePath, MimeMapping.GetMimeMapping(audio.FilePath));
        }

        //
        // GET: /Resources/Download?crid={crid}
        [OutputCache(Duration = 604800)] // 1 week
        public async Task<ActionResult> Download(long crid)
        {
            long customerId = CurrentUser.CustomerId;

            Customer customer = (await _dataService.Customer_SelectAsync(customerId));
            if (customer == null)
            {
                return new HttpNotFoundResult();
            }

            CustomerResource resource = (await _dataService.CustomerResource_SelectAsync(crid));
            if (resource.fk_CustomerId != customerId
                && resource.fk_OrganizationId != customer.fk_OrganizationId)
            {
                return new HttpNotFoundResult();
            }

            var type = (TemplateScriptItemType) resource.fk_TemplateScriptItemTypeId;
            string contentType;
            switch (type)
            {
                case TemplateScriptItemType.Text:
                    contentType = "text/plain";
                    break;
                default:
                    contentType = MimeMapping.GetMimeMapping(resource.Value);
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