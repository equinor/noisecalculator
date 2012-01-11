using NHibernate;
using Ninject.Modules;
using NoiseCalculator.Infrastructure.DataAccess.Implementations;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.Infrastructure.NHibernate;

namespace NoiseCalculator.NinjectBootstrapper
{
    public class NoiseCalculatorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactoryManager>().To<SessionFactoryManager>().InSingletonScope();
            Bind<ISession>().ToProvider<SessionProvider>().InRequestScope();
            Bind<IStatelessSession>().ToProvider<StatelessSessionProvider>().InRequestScope();

            Bind(typeof (IDAO<,>)).To(typeof (GenericDAO<,>));
            Bind<ITaskDAO>().To<TaskDAO>();
            Bind<ISelectedTaskDAO>().To<SelectedTaskDAO>();
        }
    }
}
