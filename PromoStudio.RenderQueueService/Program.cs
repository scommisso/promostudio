using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.RenderQueueService
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
                var service = new RenderQueueService();
                service.DoWork();
            }
            else
            {
                // Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new RenderQueueService() };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
