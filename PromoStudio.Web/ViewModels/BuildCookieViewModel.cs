using System.Collections.Generic;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class BuildCookieViewModel
    {
        public BuildCookieViewModel()
        {
            CompletedSteps = new List<int>();
        }

        public CustomerVideo Video { get; set; }
        public List<int> CompletedSteps { get; set; }
    }
}