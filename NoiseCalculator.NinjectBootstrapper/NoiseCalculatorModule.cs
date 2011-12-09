using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Ninject.Modules;
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
        }
    }
}
