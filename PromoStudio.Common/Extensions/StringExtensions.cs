using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PromoStudio.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToAfterEffectsPath(this string windowsPath)
        {
            if (string.IsNullOrEmpty(windowsPath)) { return null; }
            if (windowsPath[1] == ':')
            {
                windowsPath = "/" + windowsPath[0] + windowsPath.Substring(2);
            }
            windowsPath = windowsPath.Replace("\\", "/");
            return windowsPath;
        }

        private static readonly Regex _collapseUnderscoreRe = new Regex("([_-]{2,})", RegexOptions.Compiled);
        public static string ToSafeFileName(this string fileName)
        {
            var sbFileName = new StringBuilder();
            foreach (char c in fileName)
            {
                if (char.IsLetterOrDigit(c)
                    || c == '.'
                    || c == '-'
                    || c == '_')
                {
                    sbFileName.Append(c);
                }
            }
            string outFile = sbFileName.ToString();
            outFile = _collapseUnderscoreRe.Replace(outFile, new MatchEvaluator(m => "_"));
            return outFile;
        }
    }
}
