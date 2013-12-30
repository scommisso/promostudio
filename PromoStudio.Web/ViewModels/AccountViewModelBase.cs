using System.Collections.Generic;
using Newtonsoft.Json;
using PromoStudio.Common.Models;
using PromoStudio.Resources;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public abstract class AccountViewModelBase : ViewModelBase
    {
        public Customer Customer { get; set; }

        #region ctor

        public AccountViewModelBase(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
        }

        #endregion
    }
}