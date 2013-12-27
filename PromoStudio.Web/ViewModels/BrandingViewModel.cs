using PromoStudio.Common.Models;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class BrandingViewModel : BuildViewModelBase
    {
        public Organization Organization { get; set; }

        public BrandingViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 2;
        }
    }
}