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
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StockAudioId = pk_StockAudioId,
                Name = Name,
                Description = Description,
                FilePath = FilePath
            };
        }
    }
}
