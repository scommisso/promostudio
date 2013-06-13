using System;
using System.Web;
using System.Web.Mvc;

namespace PromoStudio.Web
{
    public static class UrlHelperExtension
    {
        public static string Absolute(this UrlHelper url, string relativeOrAbsolute)
        {
            var uri = new Uri(relativeOrAbsolute, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                return relativeOrAbsolute;
            }

            // At this point, we know the url is relative.
            var requestUri = url.RequestContext.HttpContext.Request.Url;
            var absolute = string.Format("{0}://{1}{2}", requestUri.Scheme, requestUri.Authority, relativeOrAbsolute);
            return absolute;
            //return VirtualPathUtility.ToAbsolute(relativeOrAbsolute);
        }
    }
}