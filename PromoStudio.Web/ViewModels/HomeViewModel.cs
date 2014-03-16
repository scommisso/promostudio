using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
        }
    }
}