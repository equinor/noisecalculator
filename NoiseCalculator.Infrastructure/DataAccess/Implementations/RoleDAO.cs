using System.Collections.Generic;
using System.Threading;
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
                .And(x => x.CultureName == Thread.CurrentThread.CurrentCulture.Name)
                .SingleOrDefault<Role>();
        }

        public IEnumerable<int> GetAreaNoiseRoleIds()
        {
            return _session.QueryOver<Role>()
                .Where(x => x.SystemTitle == "AreaNoise")
                .SelectList(x => x.Select(y => y.Id)).List<int>();
        }
    }
}