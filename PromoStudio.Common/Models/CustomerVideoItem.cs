using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Models
{
    public class CustomerVideoItem
    {
        public long pk_CustomerVideoItemId { get; set; }
        public long fk_CustomerVideoId { get; set; }
        public long fk_CustomerVideoItemId { get; set; }
        public sbyte fk_CustomerVideoItemTypeId { get; set; }
        public sbyte? SortOrder { get; set; }

        public CustomerVideoItemType Type
        {
            get { return (CustomerVideoItemType)fk_CustomerVideoItemTypeId; }
            set { fk_CustomerVideoItemTypeId = (sbyte)value; }
        }

        public CustomerTemplateScript CustomerScript { get; set; }
        public CustomerVideoVoiceOver VoiceOver { get; set; }
        public StockVideo StockVideo { get; set; }
        public StockAudio StockAudio { get; set; }

        public string GetRenderItemJson(bool isPreview)
        {
            // Video
            if (Type == CustomerVideoItemType.StockVideo && StockVideo != null)
            {
                return string.Format("{{ file: \"{0}\", includeAudio: true }}",
                    StockVideo.FilePath.ToAfterEffectsPath());
            }
            if (Type == CustomerVideoItemType.CustomerTemplateScript && CustomerScript != null
                && ((!isPreview && !string.IsNullOrEmpty(CustomerScript.CompletedFilePath))
                || (isPreview && !string.IsNullOrEmpty(CustomerScript.PreviewFilePath))))
            {
                return string.Format("{{ file: \"{0}\", includeAudio: true }}",
                    isPreview
                    ? CustomerScript.PreviewFilePath.ToAfterEffectsPath()
                    : CustomerScript.CompletedFilePath.ToAfterEffectsPath());
            }

            // Audio
            if (Type == CustomerVideoItemType.StockAudio && StockAudio != null)
            {
                return string.Format("{{ file: \"{0}\", gainAdjust: 0 }}",
                    StockAudio.FilePath.ToAfterEffectsPath());
            }
            if (Type == CustomerVideoItemType.CustomerVideoVoiceOver && VoiceOver != null && !string.IsNullOrEmpty(VoiceOver.FilePath))
            {
                return string.Format("{{ file: \"{0}\", gainAdjust: 0 }}",
                    VoiceOver.FilePath.ToAfterEffectsPath());
            }

            // Item missing data
            return null;
        }

        public dynamic ToPoco()
        {
            return new
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
