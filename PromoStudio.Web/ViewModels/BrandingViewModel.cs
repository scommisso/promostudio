using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class BrandingViewModel : BuildViewModelBase
    {
        public Organization Organization { get; set; }

        public BrandingViewModel()
            : base()
        {
            CurrentStep = 2;
        }
    }
}