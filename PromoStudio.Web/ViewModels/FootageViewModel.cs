using System.Collections.Generic;
using PromoStudio.Common.Models;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class FootageViewModel : BuildViewModelBase
    {
        public List<Storyboard> Storyboards { get; set; } 

        public FootageViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 1;
        }
    }
}