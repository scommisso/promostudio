using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.CloudStatusService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                // Console App
                var service = new CloudStatusService();
                service.DoWork();
            }
            else
            {
                // Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new CloudStatusService() };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
