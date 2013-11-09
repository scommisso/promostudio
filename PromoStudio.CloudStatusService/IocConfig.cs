using Autofac;
using log4net;
using PromoStudio.Data;
using PromoStudio.Storage;
using System.Reflection;

namespace PromoStudio.CloudStatusService
{
    public class IocConfig
    {
        public static IContainer Container { get; private set; }
        public static void RegisterIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IDataService).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(ICloudStatusProcessor).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IStorageProvider).Assembly).AsImplementedInterfaces();

            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            builder.RegisterInstance<ILog>(logger);

            Container = builder.Build();
        }
    }
}