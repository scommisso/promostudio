using System.Web.Mvc;
using log4net;
using PromoStudio.Data;
using PromoStudio.Web.ViewModels;

namespace PromoStudio.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var vm = new HomeViewModel(Request.RequestContext.HttpContext, RouteData);
            if (vm.User != null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View(vm);
        }
    }
}