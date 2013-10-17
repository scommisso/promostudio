using System.Collections.Generic;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class FootageViewModel : BuildViewModelBase
    {
        public FootageViewModel()
            : base()
        {
            CurrentStep = 1;
        }

        public List<Storyboard> Storyboards { get; set; } 
    }
}