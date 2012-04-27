using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NoiseCalculator.Infrastructure.Mapping;

namespace NoiseCalculator.Infrastructure.NHibernate
{
    public class SessionFactoryManager : ISessionFactoryManager
    {
        private const string ConnectionString = @"Data Source=st-tw466\TM614;Initial Catalog=NoiseCalculator;User Id=noisecalculator;Password=t391H_ie75;";

        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;


        public SessionFactoryManager()
        {
            _configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
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
            SchemaExport schemaExporter = new SchemaExport(_configuration);
            schemaExporter.Create(false, true);
            //schemaExporter.SetOutputFile("C:\\appl\\sql.txt");
            //schemaExporter.Execute(true, false, false);
         }
    }
}