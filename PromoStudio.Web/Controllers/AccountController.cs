using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Web.ViewModels;

namespace PromoStudio.Web.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        public AccountController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Account/
        public async Task<ActionResult> Index()
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

            var vm = new AccountDashboardViewModel(Request.RequestContext.HttpContext, RouteData)
            {
                Customer = customerInfo
            };

            return PAjax("Index", model: vm);
        }
    }
}