using System;
using System.Linq;
using PromoStudio.Web.ViewModels;

namespace PromoStudio.Web.Models.Session
{
    [Serializable]
    public class BuildState
    {
        public int[] CompletedSteps { get; set; }
        public CustomerVideo Video { get; set; }

        public BuildState()
        {
        }

        public BuildState(BuildCookieViewModel cookieViewModel)
        {
            CompletedSteps = cookieViewModel.CompletedSteps.ToArray();
            Video = new CustomerVideo(cookieViewModel.Video);
        }

        public BuildCookieViewModel ToModel()
        {
            return new BuildCookieViewModel()
            {
                CompletedSteps = CompletedSteps.ToList(),
                Video = Video.ToModel()
            };
        }
    }
}