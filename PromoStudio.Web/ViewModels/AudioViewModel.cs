using System.Collections.Generic;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class AudioViewModel : BuildViewModelBase
    {
        public List<StockAudio> StockAudio { get; set; }
        public List<VoiceActor> VoiceActors { get; set; }

        public AudioViewModel()
            : base()
        {
            CurrentStep = 4;
        }
    }
}