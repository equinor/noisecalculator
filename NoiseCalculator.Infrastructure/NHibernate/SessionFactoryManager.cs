using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NoiseCalculator.Infrastructure.Mapping;
using Configuration = NHibernate.Cfg.Configuration;

namespace NoiseCalculator.Infrastructure.NHibernate
{
    public class SessionFactoryManager : ISessionFactoryManager
    {
        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;


        public  SessionFactoryManager()
        {
            var result = ConfigurationManager.ConnectionStrings["ConnectionString"];

            var connectionString = result.ToString();
            
            _configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TaskMap>())
                .BuildConfiguration();
            
            _sessionFactory = _configuration.BuildSessionFactory();
        }
        
        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }

        public IStatelessSession OpenStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession();
        }

        public void ExportSchema()
        {
            var schemaExporter = new SchemaExport(_configuration);
            schemaExporter.Create(false, true);
            //schemaExporter.SetOutputFile("C:\\appl\\sql.txt");
            //schemaExporter.Execute(true, false, false);
         }
    }
}