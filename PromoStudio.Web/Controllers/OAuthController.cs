using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Web.Properties;

namespace PromoStudio.Web.Controllers
{
    public class OAuthController : AsyncController
    {
        private IDataService _dataService;

        public OAuthController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public async Task<ActionResult> Authorize(sbyte pId, string key, string name, string email)
        {
            try
            {
                var customer = await _dataService.Customer_SelectAsyncByLoginCredential(pId, key);
                bool forcePrimary = false;
                if (customer == null)
                {
                    customer = new Customer()
                    {
                        FullName = name,
                        fk_OrganizationId = 1,
                        fk_CustomerStatusId = (sbyte)CustomerStatus.Active,
                        DateCreated = DateTime.Now
                    };
                    customer = await _dataService.Customer_InsertAsync(customer);
                    forcePrimary = true;
                }
                else
                {
                    // TODO: determine if login should be primary (maybe add login stuff to customerselectbylogin?)
                }
                var loginInfo = new CustomerLoginCredential()
                {
                    fk_CustomerId = customer.pk_CustomerId,
                    fk_CustomerLoginPlatformId = pId,
                    EmailAddress = email,
                    LoginKey = key,
                    LastLogin = DateTime.Now,
                    PrimaryLogin = (sbyte)(forcePrimary ? 1 : 0)
                };
                _dataService.CustomerLoginCredential_InsertUpdate(loginInfo);

                // TODO: Add login cookie

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                // TODO: Log
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    
        public ActionResult GetFacebookAccessToken() {

            // TODO: Add memory cache lookup for this since it doesn't expire often

            try
            {
                var wc = new WebClient();
                string result = wc.DownloadString(new Uri("https://graph.facebook.com/oauth/access_token" +
                    "?client_id=" + Url.Encode(Settings.Default.FacebookClientId) +
                    "&client_secret=" + Url.Encode(Settings.Default.FacebookClientSecret) +
                    "&grant_type=client_credentials"));

                if (!string.IsNullOrEmpty(result) && result.Length > 13 && result.Substring(0, 13).ToLower() == "access_token=")
                {
                    return Json(new { access_token = result.Substring(13) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "No access key returned" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error retrieving access key" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
