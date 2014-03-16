using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class StockAudio
    {
        public int pk_StockAudioId { get; set; }
        public sbyte fk_StockItemStatusId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public StockItemStatus Status
        {
            get { return (StockItemStatus) fk_StockItemStatusId; }
            set { fk_StockItemStatusId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StockAudioId,
                fk_StockItemStatusId,
                fk_OrganizationId,
                fk_VerticalId,
                Name,
                Description,
                FilePath,
                DateCreated,
                DateUpdated
            };
        }
    }
}