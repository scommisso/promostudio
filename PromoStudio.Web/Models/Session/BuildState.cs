using System;
using System.Linq;
using PromoStudio.Web.ViewModels;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class BuildState
    {
        public BuildState()
        {
        }

        public BuildState(BuildCookieViewModel cookieViewModel)
        {
            CompletedSteps = cookieViewModel.CompletedSteps.ToArray();
            Video = new CustomerVideo(cookieViewModel.Video);
        }

        public int[] CompletedSteps { get; set; }
        public CustomerVideo Video { get; set; }

        public BuildCookieViewModel ToModel()
        {
            return new BuildCookieViewModel
            {
                CompletedSteps = CompletedSteps.ToList(),
                Video = Video.ToModel()
            };
        }
    }
}