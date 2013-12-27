using System.Collections.Generic;
using PromoStudio.Common.Models;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class AudioViewModel : BuildViewModelBase
    {
        public List<StockAudio> StockAudio { get; set; }
        public List<VoiceActor> VoiceActors { get; set; }

        public AudioViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 4;
        }
    }
}