using System;
using System.ServiceProcess;

namespace PromoStudio.CloudStatusService
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
                var service = new CloudStatusService();
                service.DoWork();
            }
            else
            {
                // Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] {new CloudStatusService()};
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}