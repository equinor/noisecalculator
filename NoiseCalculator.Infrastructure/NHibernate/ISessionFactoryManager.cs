using System.Collections.Generic;
using System.Linq;
using System.Text;
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
