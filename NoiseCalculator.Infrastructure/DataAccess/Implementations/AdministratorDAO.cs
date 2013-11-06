using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class AdministratorDAO : GenericDAO<Administrator, string>, IAdministratorDAO
    {
        public AdministratorDAO(ISession session) : base(session)
        {
        }

        public bool UserIsAdmin(string username)
        {
            var admin = _session.Get<Administrator>(username);

            return admin != null;
        }
    }
}