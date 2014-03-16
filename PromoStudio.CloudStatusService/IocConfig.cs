using System.Reflection;
using Autofac;
using log4net;
using PromoStudio.Data;
using PromoStudio.Storage;
using VimeoDotNet;

namespace PromoStudio.CloudStatusService
{
    public class IocConfig
    {
        public static IContainer Container { get; private set; }

        public static void RegisterIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof (IDataService).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (ICloudStatusProcessor).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (IStreamingProvider).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (IVimeoClientFactory).Assembly).AsImplementedInterfaces();

            ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            builder.RegisterInstance(logger);

            Container = builder.Build();
        }
    }
}