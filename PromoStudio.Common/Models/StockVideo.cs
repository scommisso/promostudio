using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromoStudio.Common.Models
{
    public class StockVideo
    {
        public long pk_StockVideoId { get; set; }
        public sbyte fk_StockItemStatusId { get; set; }
        public short fk_StoryboardItemType { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public StockItemStatus Status
        {
            get { return (StockItemStatus)fk_StockItemStatusId; }
            set { fk_StockItemStatusId = (sbyte)value; }
        }

        public StoryboardItemType StoryboardType
        {
            get { return (StoryboardItemType)fk_StoryboardItemType; }
            set { fk_StoryboardItemType = (short)value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StockVideoId = pk_StockVideoId,
                fk_StockItemStatusId = fk_StockItemStatusId,
                fk_StoryboardItemType = fk_StoryboardItemType,
                fk_OrganizationId = fk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                Description = Description,
                FilePath = FilePath,
                DateCreated = DateCreated,
                DateUpdated = DateUpdated
            };
        }
    }
}
