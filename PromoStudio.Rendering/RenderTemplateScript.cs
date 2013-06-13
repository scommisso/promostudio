using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Extensions;
using PromoStudio.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Rendering
{
    public class RenderTemplateScript : ScriptBase
    {
        public override string ScriptTemplateFileName
        {
            get
            {
                return "TemplateVideo.js";
            }
        }

        public RenderTemplateScript(CustomerTemplateScript script, string outputPath, bool renderAsPreview)
        {
            Replacements.Add("ProjectPath", script.Template.ProjectFilePath.ToAfterEffectsPath());
            Replacements.Add("OutputPath", outputPath.ToAfterEffectsPath());
            Replacements.Add("RenderComp", renderAsPreview ? script.Template.RenderPreviewCompName : script.Template.RenderCompName);
            Replacements.Add("RenderStart", script.Template.RenderCompStartTime.ToString());
            Replacements.Add("RenderDuration", (script.Template.RenderCompEndTime - script.Template.RenderCompStartTime).ToString());
            Replacements.Add("RenderTemplate", GetRenderTemplate(renderAsPreview));
            Replacements.Add("Swaps", script.GetSwapItemJson());
        }
    }
}
