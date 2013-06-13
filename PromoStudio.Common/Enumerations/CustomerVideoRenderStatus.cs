using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Enumerations
{
    public enum CustomerVideoRenderStatus : sbyte
    {
        Pending = 1,
        InProgressTemplatePreview,
        CompletedTemplatePreview,
        InProgressVideoPreview,
        CompletedVideoPreview,
        UploadingPreview,
        PendingVoiceTalent,
        CompletedVoiceTalent,
        InProgressTemplate,
        CompletedTemplate,
        InProgressFinalRender,
        CompletedFinalRender,
        UploadingFinalRender,
        Completed,
        Canceled
    }
}
