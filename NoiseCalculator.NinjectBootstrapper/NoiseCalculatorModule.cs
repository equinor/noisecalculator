using NHibernate;
using Ninject.Modules;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Infrastructure.DataAccess.Implementations;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.Infrastructure.NHibernate;
using NoiseCalculator.Infrastructure.Pdf;

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
            Bind<IHelicopterTaskDAO>().To<HelicopterTaskDAO>();
            Bind<IHelicopterNoiseProtectionDAO>().To<HelicopterNoiseProtectionDAO>();
            Bind<IRoleDAO>().To<RoleDAO>();
            Bind<IRotationDAO>().To<RotationDAO>();
            Bind<INoiseProtectionDAO>().To<NoiseProtectionDAO>();

            Bind<IPdfExporter>().To<PdfExporterGios>();

            Bind<INoiseLevelService>().To<NoiseLevelService>();
        }
    }
}
