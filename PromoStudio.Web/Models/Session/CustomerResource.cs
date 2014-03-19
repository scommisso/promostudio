using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerResource
    {
        public CustomerResource()
        {
        }

        public CustomerResource(Common.Models.CustomerResource resource)
        {
            pk_CustomerResourceId = resource.pk_CustomerResourceId;
            fk_CustomerId = resource.fk_CustomerId.HasValue ? resource.fk_CustomerId.Value : 0;
            fk_TemplateScriptItemTypeId = resource.fk_TemplateScriptItemTypeId;
            fk_TemplateScriptItemCategoryId = resource.fk_TemplateScriptItemCategoryId;
            fk_CustomerResourceStatusId = resource.fk_CustomerResourceStatusId;
            Value = resource.Value;
            OriginalFileName = resource.OriginalFileName;
            ThumbnailUrl = resource.ThumbnailUrl;
        }

        public long pk_CustomerResourceId { get; set; }
        public long fk_CustomerId { get; set; }
        public sbyte fk_TemplateScriptItemTypeId { get; set; }
        public short fk_TemplateScriptItemCategoryId { get; set; }
        public sbyte fk_CustomerResourceStatusId { get; set; }
        public string Value { get; set; }
        public string OriginalFileName { get; set; }
        public string ThumbnailUrl { get; set; }

        public Common.Models.CustomerResource ToModel()
        {
            return new Common.Models.CustomerResource
            {
                pk_CustomerResourceId = pk_CustomerResourceId,
                fk_CustomerId = fk_CustomerId,
                fk_TemplateScriptItemTypeId = fk_TemplateScriptItemTypeId,
                fk_TemplateScriptItemCategoryId = fk_TemplateScriptItemCategoryId,
                fk_CustomerResourceStatusId = fk_CustomerResourceStatusId,
                Value = Value,
                OriginalFileName = OriginalFileName,
                ThumbnailUrl = ThumbnailUrl
            };
        }
    }
}