using NHibernate;
using Ninject.Modules;
using Ninject.Web.Common;
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
            Bind<IAdministratorDAO>().To<AdministratorDAO>();
            Bind<IHelicopterNoiseProtectionDAO>().To<HelicopterNoiseProtectionDAO>();
            Bind<IHelicopterTaskDAO>().To<HelicopterTaskDAO>();
            Bind<INoiseProtectionDAO>().To<NoiseProtectionDAO>();
            Bind<IRoleDAO>().To<RoleDAO>();
            Bind<IRotationDAO>().To<RotationDAO>();
            Bind<ISelectedTaskDAO>().To<SelectedTaskDAO>();
            Bind<ITaskDAO>().To<TaskDAO>();
            Bind<ITaskDefinitionDAO>().To<TaskDefinitionDAO>();
            
            // PDF Exporter
            Bind<IPdfExporter>().To<PdfExporterGios>();
            Bind<INoiseLevelService>().To<NoiseLevelService>();
        }
    }
}
