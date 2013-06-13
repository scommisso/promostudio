using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Autofac;
using Autofac.Integration.Mvc;
using PromoStudio.Data;

namespace PromoStudio.Web
{
    public class IocConfig
    {
        public static IContainer Container { get; private set; }
        public static void RegisterIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(typeof(IDataService).Assembly).AsImplementedInterfaces();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}