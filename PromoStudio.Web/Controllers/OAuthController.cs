using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
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
                var clcs = await _dataService.CustomerWithLoginCredential_SelectAsyncByLoginCredential(pId, key, email);
                var clc = clcs.FirstOrDefault(c => c.fk_CustomerLoginPlatformId == pId);
                if (clc == null) { clc = clcs.FirstOrDefault(); }

                Customer customer = null;
                Organization organization = null;
                bool forcePrimary = false;
                if (clc == null)
                {
                    customer = new Customer()
                    {
                        FullName = name,
                        fk_OrganizationId = null,
                        fk_CustomerStatusId = (sbyte)CustomerStatus.Active,
                        DateCreated = DateTime.Now
                    };
                    customer = await _dataService.Customer_InsertAsync(customer);
                    forcePrimary = true;
                }
                else
                {
                    customer = clc.ToCustomer();
                    organization = clc.ToOrganization();
                    forcePrimary = (clc.PrimaryLogin == 1);
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

                CreateCookie(Response, customer, organization, loginInfo);

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

        private void CreateCookie(HttpResponseBase response, Customer customer, Organization organization, CustomerLoginCredential loginInfo)
        {
            var authData = new AuthData()
            {
                AuthenticationType = loginInfo.Platform.ToString() + " OAuth",
                CustomerId = customer.pk_CustomerId,
                EmailAddress = loginInfo.EmailAddress,
                FullName = customer.FullName,
                OrganizationId = organization == null ? (long?) null : organization.pk_OrganizationId,
                OrganizationName = organization == null ? (string) null : organization.DisplayName
            };

            var ticket = new FormsAuthenticationTicket(
                1,
                authData.EmailAddress,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                true,
                JsonConvert.SerializeObject(authData));
            var encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

            response.Cookies.Add(cookie);
        }
    }
}
