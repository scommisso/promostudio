using System.Security.Principal;

namespace PromoStudio.Web
{
    public class PromoStudioIdentity : IPromoStudioIdentity, IIdentity
    {
        public PromoStudioIdentity(AuthData authData)
        {
            CustomerId = authData.CustomerId;
            OrganizationId = authData.OrganizationId;
            VerticalId = authData.VerticalId;
            FullName = authData.FullName;
            EmailAddress = authData.EmailAddress;
            OrganizationName = authData.OrganizationName;
            AuthenticationType = authData.AuthenticationType;
        }

        public long CustomerId { get; set; }
        public int? OrganizationId { get; set; }
        public int? VerticalId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string OrganizationName { get; set; }
        public string AuthenticationType { get; set; }

        string IIdentity.Name
        {
            get { return EmailAddress; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(AuthenticationType); }
        }
    }
}