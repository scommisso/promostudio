using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public abstract class ViewModelBase
    {
        public PromoStudioIdentity User { get; private set; }
        public string UserFirstName { get; private set; }
        public bool IsAccountController { get; private set; }
        public bool IsBuildController { get; private set; }

        public ViewModelBase(HttpContextBase context, RouteData routeData)
        {
            LoadIdentity(context);
            LoadNavigationFlags(routeData);
        }

        private void LoadIdentity(HttpContextBase context)
        {
            User = context.User != null
                && context.User.Identity != null
                && context.User.Identity.IsAuthenticated
                    ? context.User.Identity as PromoStudioIdentity
                    : null;

            if (User != null)
            {
                string firstName = User.FullName;
                int ix = firstName.IndexOf(" ");
                if (ix > 0)
                {
                    firstName = firstName.Substring(0, ix);
                }
                UserFirstName = firstName;
            }
        }

        private void LoadNavigationFlags(RouteData routeData)
        {
            IsAccountController = string.Compare((string)routeData.Values["controller"], "Account", StringComparison.OrdinalIgnoreCase) == 0;
            IsBuildController = string.Compare((string)routeData.Values["controller"], "Build", StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}