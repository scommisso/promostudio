using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using PromoStudio.Common.Enumerations;
using PromoStudio.Rendering.Properties;

namespace PromoStudio.Rendering
{
    public abstract class ScriptBase
    {
        private readonly Dictionary<string, string> _replacements = new Dictionary<string, string>();

        public string ScriptTemplateDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScriptTemplates");
            }
        }

        public string ScriptOutputDirectory
        {
            get { return Path.Combine(Settings.Default.ScriptPath, "RenderScripts"); }
        }

        public IDictionary<string, string> Replacements
        {
            get { return _replacements; }
        }

        /// <summary>
        ///     Returns the name of the input script template file
        /// </summary>
        public abstract string ScriptTemplateFileName { get; }

        /// <summary>
        ///     Returns the name path to the output script template file
        /// </summary>
        public string ScriptTemplatePath
        {
            get { return Path.Combine(ScriptTemplateDirectory, ScriptTemplateFileName); }
        }

        public void Render()
        {
            string path = null;
            try
            {
                path = GenerateScript();
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    throw new ApplicationException("Error generating script for template: " + GetType().Name);
                }
                var process = new AeProcess();
                process.ExecuteProcess(path);
            }
            catch (Exception ex)
            {
                // TODO: Log
                throw;
            }
            finally
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                    // TODO: Log
                }
            }
        }

        /// <summary>
        ///     Generate the script and return the path to the completed .jsx script file.
        /// </summary>
        /// <returns>The path to the .jsx script file (delete after use)</returns>
        public string GenerateScript()
        {
            string template = File.ReadAllText(ScriptTemplatePath);
            string fileName = string.Format("{0:N}.jsx", Guid.NewGuid());

            if (!Directory.Exists(ScriptOutputDirectory))
            {
                Directory.CreateDirectory(ScriptOutputDirectory);
            }

            string filePath = Path.Combine(ScriptOutputDirectory, fileName);

            foreach (var replacement in Replacements)
            {
                template = template.Replace(string.Format("{{{{{0}}}}}", replacement.Key.ToUpper()), replacement.Value);
            }

            File.WriteAllText(filePath, template, Encoding.UTF8);

            return filePath;
        }

        protected string GetRenderTemplate(bool isPreview)
        {
            RenderTemplate t = isPreview
                ? RenderTemplate.NTSC_H264
                : RenderTemplate.FullHD_H264;
            return t.ToString().Replace("_", "-");
        }
    }
}