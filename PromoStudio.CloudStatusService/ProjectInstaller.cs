using System.ComponentModel;
using System.Configuration.Install;

namespace PromoStudio.CloudStatusService
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