using Ninject.Modules;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;

namespace NoiseCalculator.UI.Web.Ninject
{
    public class NoiseCalculatorWebModule : NinjectModule
    {
        public override void Load()
        {
            // Bindings for Application Serivces
            Bind<IAdministratorService>().To<AdministratorService>();
            Bind<IGenericTaskDefinitionService>().To<GenericTaskDefinitionService>();
            Bind<IGenericTaskService>().To<GenericTaskService>();
            Bind<IHelicopterNoiseProtectionDefinitionService>().To<HelicopterNoiseProtectionDefinitionService>();
            Bind<IHelicopterNoiseProtectionService>().To<HelicopterNoiseProtectionService>();
            Bind<IHelicopterTaskService>().To<HelicopterTaskService>();
            Bind<IHelicopterTypeService>().To<HelicopterTypeService>();
            Bind<IHelicopterWorkIntervalService>().To<HelicopterWorkIntervalService>();
            Bind<INoiseProtectionDefinitionService>().To<NoiseProtectionDefinitionService>();
            Bind<INoiseProtectionService>().To<NoiseProtectionService>();
            Bind<IRotationTaskDefinitionService>().To<RotationTaskDefinitionService>();
            Bind<IRotationTaskService>().To<RotationTaskService>();
            Bind<ITaskDefinitionService>().To<TaskDefinitionService>();
        }
    }
}