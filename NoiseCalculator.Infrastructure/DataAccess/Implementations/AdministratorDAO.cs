using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class AdministratorDAO : IAdministratorDAO
    {
        private readonly ISession _session;

        public AdministratorDAO(ISession session)
        {
            _session = session;
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