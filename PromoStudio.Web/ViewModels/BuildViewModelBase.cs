using System.Collections.Generic;
using Newtonsoft.Json;
using PromoStudio.Common.Models;
using PromoStudio.Resources;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public abstract class BuildViewModelBase : ViewModelBase
    {
        #region Constants
        private static readonly string[] _keys = new string[]
        {
            "Footage",
            "Customize",
            "Script",
            "Audio",
            "Preview"
        };
        private static readonly string[] _titles = new string[]
        {
            Strings.BuildStep__Footage,
            Strings.BuildStep__Customize,
            Strings.BuildStep__Script,
            Strings.BuildStep__Audio,
            Strings.BuildStep__Preview
        };
        private static readonly string[] _descriptions = new string[]
        {
            Strings.BuildStep__Choose_your_template,
            Strings.BuildStep__Customize_the_footage,
            Strings.BuildStep__Add_your_script,
            Strings.BuildStep__Select_your_Audio,
            Strings.BuildStep__Review_your_video
        };
        private static readonly string[] _classes = new string[]
        {
            "footage",
            "customize",
            "script",
            "audio",
            "preview"
        };
        #endregion

        public List<int> StepsCompleted { get; set; }
        public int CurrentStep { get; set; }
        public CustomerVideo Video { get; set; }

        [JsonIgnore]
        public string VmJson { get; set; }

        #region ctor

        public BuildViewModelBase(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            StepsCompleted = new List<int>();
        }

        #endregion

        public bool IsStepCompleted(int stepId)
        {
            return StepsCompleted.Contains(stepId);
        }

        public bool IsStepAvailable(int stepId)
        {
            if (stepId < 1 || stepId > 5)
            {
                return false;

            }
            if (!IsStepCompleted(stepId))
            {
                for (int i = 1; i < stepId; i++)
                {
                    if (!IsStepCompleted(i))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string GetStepKey(int stepId)
        {
            if (stepId < 1 || stepId > 5)
            {
                return null;
            }
            return _keys[stepId - 1];
        }

        public string GetStepTitle(int stepId)
        {
            if (stepId < 1 || stepId > 5)
            {
                return null;
            }
            return _titles[stepId - 1];
        }

        public string GetStepClass(int stepId)
        {
            if (stepId < 1 || stepId > 5)
            {
                return null;
            }
            return _classes[stepId - 1];
        }

        public string GetStepDescription(int stepId)
        {
            if (stepId < 1 || stepId > 5)
            {
                return null;
            }
            return _descriptions[stepId - 1];
        }

        public string GetFooterClassAttribute(int stepId)
        {
            var classes = new List<string>();
            if (stepId > CurrentStep)
            {
                if (!IsStepAvailable(stepId))
                {
                    classes.Add("notready");
                }
            }

            if (classes.Count == 0) { return ""; }
            return string.Format(" class=\"{0}\"", string.Join(" ", classes));
        }

        public string GetNavClassAttribute(int stepId)
        {
            var classes = new List<string>();
            if (stepId == CurrentStep)
            {
                classes.Add("active");
            }
            if (stepId > CurrentStep)
            {
                if (!IsStepAvailable(stepId))
                {
                    classes.Add("notready");
                }
            }

            if (classes.Count == 0) { return ""; }
            return string.Format(" class=\"{0}\"", string.Join(" ", classes));
        }
    }
}