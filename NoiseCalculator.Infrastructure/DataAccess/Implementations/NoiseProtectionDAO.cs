using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class NoiseProtectionDAO : GenericDAO<NoiseProtection,int>, INoiseProtectionDAO
    {
        public NoiseProtectionDAO(ISession session) : base(session)
        {
        }

        public IEnumerable<NoiseProtection> GetAllByCultureName(string cultureName)
        {
            var queryOver = _session.QueryOver<NoiseProtection>().OrderBy(x => x.CultureName).Asc;

            if(string.IsNullOrEmpty(cultureName) == false)
            {
                queryOver.Where(x => x.CultureName == cultureName);
            }

            return queryOver.List<NoiseProtection>();
        }
    }
}