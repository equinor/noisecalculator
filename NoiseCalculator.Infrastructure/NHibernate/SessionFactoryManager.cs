using System;
using System.Threading;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
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
        //private const string ConnectionString = "Data Source=localhost;Initial Catalog=NoiseCalculator;Integrated Security=SSPI;";
        private const string ConnectionString = @"Data Source=st-tw466\TM614;Initial Catalog=NoiseCalculator;User Id=noisecalculator;Password=t391H_ie75;";

        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;


        public SessionFactoryManager()
        {
            NHibernateProfiler.Initialize();

            _configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TaskMap>())
                .ProxyFactoryFactory<ProxyFactoryFactory>()
                .BuildConfiguration();

            _sessionFactory = _configuration.BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            ISession session = _sessionFactory.OpenSession();
            //session.EnableFilter("CultureNameFilter").SetParameter("cultureName", Thread.CurrentThread.CurrentCulture.Name);
            return session;
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