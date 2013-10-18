using System;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class CustomerVideoItem
    {
        public long pk_CustomerVideoItemId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public long fk_CustomerVideoItemId { get; set; }
        public sbyte fk_CustomerVideoItemTypeId { get; set; }
        public sbyte? SortOrder { get; set; }

        public CustomerVideoItem()
        {
        }

        public CustomerVideoItem(Common.Models.CustomerVideoItem videoItem)
        {
            pk_CustomerVideoItemId = videoItem.pk_CustomerVideoItemId;
            fk_CustomerVideoId = videoItem.fk_CustomerVideoId;
            fk_CustomerVideoItemId = videoItem.fk_CustomerVideoItemId;
            fk_CustomerVideoItemTypeId = videoItem.fk_CustomerVideoItemTypeId;
            SortOrder = videoItem.SortOrder;
        }

        public Common.Models.CustomerVideoItem ToModel()
        {
            return new Common.Models.CustomerVideoItem()
            {
                pk_CustomerVideoItemId = pk_CustomerVideoItemId,
                fk_CustomerVideoId = fk_CustomerVideoId,
                fk_CustomerVideoItemId = fk_CustomerVideoItemId,
                fk_CustomerVideoItemTypeId = fk_CustomerVideoItemTypeId,
                SortOrder = SortOrder
            };
        }
    }
}