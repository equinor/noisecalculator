using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NoiseCalculator.Infrastructure.Mapping;

namespace NoiseCalculator.Infrastructure.NHibernate
{
    public class SessionFactoryManager : ISessionFactoryManager
    {
        // Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;
        // Data Source=myServerAddress;Initial Catalog=myDataBase;Integrated Security=SSPI;
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=NoiseCalculator;Integrated Security=SSPI;";

        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;


        public SessionFactoryManager()
        {
            _configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TaskMap>())
                .ProxyFactoryFactory<ProxyFactoryFactory>()
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
        }
    }
}