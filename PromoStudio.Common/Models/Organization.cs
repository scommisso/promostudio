using System;

namespace PromoStudio.Common.Models
{
    public class Organization
    {
        public int pk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_OrganizationId = pk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                DisplayName = DisplayName,
                ContactPhone = ContactPhone,
                ContactEmail = ContactEmail,
                Website = Website,
                DateCreated = DateCreated,
                DateUpdated = DateUpdated
            };
        }
    }
}
