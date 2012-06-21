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
            Administrator admin = _session.Get<Administrator>(username);

            if(admin == null)
            {
                return false;
            }

            return true;
        }
    }
}