using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Criterion;
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
            ISessionFactoryManager sessionFactoryManager = kernel.Get<ISessionFactoryManager>();
            NHibernateProfiler.Initialize();
            ISession session = sessionFactoryManager.OpenSession();

            //SELECT *
            //FROM Task
            //WHERE id NOT IN (
            //    SELECT Id FROM Task WHERE CultureName = 'nb-NO')


            //var subQuery = QueryOver.Of<Task>().Where(x => x.CultureName == "nb-NO").Select(x => x.Id);
            //var list = session.QueryOver<Task>()
            //    .Where(Subqueries.WhereProperty<Task>(x => x.Id).NotIn(subQuery)).List();

            //string lol = "lol";


            //var detachedSubCriteria = QueryOver.Of<Task>().Where(x => x.CultureName == "nb-NO").Select(x => x.Id).DetachedCriteria;
            //var listOldschool = session.QueryOver<Task>()
            //    .Where(Subqueries.PropertyNotIn("Id", detachedSubCriteria)).List();




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