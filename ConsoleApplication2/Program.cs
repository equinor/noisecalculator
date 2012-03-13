using Ninject;
using NoiseCalculator.Infrastructure.NHibernate;
using NoiseCalculator.NinjectBootstrapper;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NoiseCalculatorModule());
            //ISessionFactoryManager sessionFactoryManager = kernel.Get<ISessionFactoryManager>();
            //sessionFactoryManager.ExportSchema();

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");

            //IDAO<Task, int> taskDAO = kernel.Get<IDAO<Task, int>>();
            //var entity = taskDAO.Get(15);
            //var list = _taskDAO.GetAllChronologically("STATOIL-NET\\LAKHA", DateTime.Now);


            //ITaskDAO taskDAO = kernel.Get<ITaskDAO>();            
            //var task = taskDAO.Get(15);

            //IRoleDAO roleDAO = kernel.Get<IRoleDAO>();
            //var role = roleDAO.Get(1);

            //IDAO<NoiseProtection, int> noiseProtectionDAO = kernel.Get<IDAO<NoiseProtection, int>>();
            //var noiseProtection = noiseProtectionDAO.Get(1);

            //IDAO<HelicopterNoiseProtection, int> helicopterNoiseProtectionDAO = kernel.Get<IDAO<HelicopterNoiseProtection, int>>();
            //var noiseProtection = helicopterNoiseProtectionDAO.Get(1);

        }
    }
}