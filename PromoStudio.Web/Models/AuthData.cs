namespace PromoStudio.Web
{
    public class AuthData
    {
        public long CustomerId { get; set; }
        public int? OrganizationId { get; set; }
        public int? VerticalId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string OrganizationName { get; set; }
        public string AuthenticationType { get; set; }
    }
}