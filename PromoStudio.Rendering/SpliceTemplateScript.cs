using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Extensions;
using PromoStudio.Common.Models;
using PromoStudio.Rendering.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Rendering
{
    public class SpliceTemplateScript : ScriptBase
    {
        public override string ScriptTemplateFileName
        {
            get
            {
                return "SpliceVideo.js";
            }
        }

        public SpliceTemplateScript(CustomerVideo video, string outputPath, bool renderAsPreview)
        {
            Replacements.Add("ProjectPath", Settings.Default.RenderProjectPath.ToAfterEffectsPath());
            Replacements.Add("OutputPath", outputPath.ToAfterEffectsPath());
            Replacements.Add("RenderComp", renderAsPreview ? Settings.Default.RenderProjectPreviewComp : Settings.Default.RenderProjectComp);
            Replacements.Add("RenderTemplate", GetRenderTemplate(renderAsPreview));
            Replacements.Add("Video", video.GetVideoItemsJson(renderAsPreview));
            Replacements.Add("Audio", video.GetAudioItemsJson());
        }
    }
}
