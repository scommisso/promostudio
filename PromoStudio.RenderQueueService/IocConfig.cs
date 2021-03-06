﻿using System.Reflection;
using Autofac;
using log4net;
using PromoStudio.Data;
using PromoStudio.RenderQueue;
using PromoStudio.Storage;

namespace PromoStudio.RenderQueueService
{
    public class IocConfig
    {
        public static IContainer Container { get; private set; }
        public static void RegisterIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IDataService).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IQueueProcessor).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IStorageProvider).Assembly).AsImplementedInterfaces();

            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            builder.RegisterInstance<ILog>(logger);

            Container = builder.Build();
        }
    }
}