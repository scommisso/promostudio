using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class SummaryViewModel : BuildViewModelBase
    {
        public SummaryViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 5;
        }
    }
}