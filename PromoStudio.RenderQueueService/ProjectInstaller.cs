using System.ComponentModel;
using System.Configuration.Install;

namespace PromoStudio.RenderQueueService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}