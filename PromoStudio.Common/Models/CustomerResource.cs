using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class CustomerResource
    {
        public long pk_CustomerResourceId { get; set; }
        public long? fk_CustomerId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public sbyte fk_TemplateScriptItemTypeId { get; set; }
        public short fk_TemplateScriptItemCategoryId { get; set; }
        public sbyte fk_CustomerResourceStatusId { get; set; }
        public string Value { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public TemplateScriptItemType Type
        {
            get { return (TemplateScriptItemType) fk_TemplateScriptItemTypeId; }
            set { fk_TemplateScriptItemTypeId = (sbyte) value; }
        }

        public TemplateScriptItemCategory Category
        {
            get { return (TemplateScriptItemCategory) fk_TemplateScriptItemCategoryId; }
            set { fk_TemplateScriptItemCategoryId = (short) value; }
        }

        public CustomerResourceStatus Status
        {
            get { return (CustomerResourceStatus) fk_CustomerResourceStatusId; }
            set { fk_CustomerResourceStatusId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerResourceId,
                fk_CustomerId,
                fk_OrganizationId,
                fk_TemplateScriptItemTypeId,
                fk_TemplateScriptItemCategoryId,
                fk_CustomerResourceStatusId,
                Value,
                DateCreated,
                DateUpdated
            };
        }
    }
}