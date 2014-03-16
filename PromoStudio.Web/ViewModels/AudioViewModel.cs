using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class AudioViewModel : BuildViewModelBase
    {
        public AudioViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 4;
        }

        public List<StockAudio> StockAudio { get; set; }
        public List<VoiceActor> VoiceActors { get; set; }
    }
}