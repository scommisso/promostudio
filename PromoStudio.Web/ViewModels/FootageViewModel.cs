using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class FootageViewModel : BuildViewModelBase
    {
        public FootageViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 1;
        }

        public List<Storyboard> Storyboards { get; set; }
    }
}