using NHibernate;

namespace NoiseCalculator.Infrastructure.NHibernate
{
    public interface ISessionFactoryManager
    {
        ISession OpenSession();
        IStatelessSession OpenStatelessSession();
        void ExportSchema();
    }
}
