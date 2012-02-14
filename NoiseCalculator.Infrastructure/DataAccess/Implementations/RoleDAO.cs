using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class RoleDAO : GenericDAO<Role, int>, IRoleDAO
    {
        public RoleDAO(ISession session) : base(session)
        {
        }

        public Role Get(string systemTitle)
        {
            return _session.QueryOver<Role>()
                .Where(x => x.SystemTitle == systemTitle)
                .SingleOrDefault<Role>();
        }
    }
}