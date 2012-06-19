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
        private const string ConnectionString = @"Data Source=st-pm611\pm611;Initial Catalog=NoiseCalculator;User Id=noisecalculator;Password=sT3%9gyL2c;";
        //private const string ConnectionString = @"Data Source=st-tw466\TM614;Initial Catalog=NoiseCalculator;User Id=noisecalculator;Password=t391H_ie75;";
        //private const string ConnectionString = @"Data Source=st-qm611\QM611;Initial Catalog=NoiseCalculator;User Id=noisecalculator;Password=4oiW!pR4v2;";
        //private const string ConnectionString = @"Data Source=localhost;Initial Catalog=NoiseCalculator;Integrated Security=SSPI;";
        
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