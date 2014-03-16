using System;
using System.ServiceProcess;

namespace PromoStudio.RenderQueueService
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
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
                ServicesToRun = new ServiceBase[] {new RenderQueueService()};
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}