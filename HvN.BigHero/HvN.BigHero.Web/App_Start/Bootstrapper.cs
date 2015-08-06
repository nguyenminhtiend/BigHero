using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using HvN.BigHero.DAL.DataContext;
using HvN.BigHero.DAL.Repository;
using HvN.BigHero.DAL.UnitOfWork;

namespace HvN.BigHero.Web
{
    public class Bootstrapper
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterType<BigHeroContext>().As<DbContext>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("HvN.BigHero.DAL"))
                  .Where(x => x.Name.EndsWith("Service"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}