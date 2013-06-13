using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromoStudio.Common.Models
{
    public class Organization
    {
        public int pk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_OrganizationId = pk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                DisplayName = DisplayName
            };
        }
    }
}
