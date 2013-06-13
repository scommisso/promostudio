using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromoStudio.Web
{
    public class AuthData
    {
        public long CustomerId { get; set; }
        public long? OrganizationId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string OrganizationName { get; set; }
        public string AuthenticationType { get; set; }
    }
}