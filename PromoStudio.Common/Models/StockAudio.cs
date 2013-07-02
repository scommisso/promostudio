using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromoStudio.Common.Models
{
    public class StockAudio
    {
        public long pk_StockAudioId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StockAudioId = pk_StockAudioId,
                fk_OrganizationId = fk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                Description = Description,
                FilePath = FilePath
            };
        }
    }
}
