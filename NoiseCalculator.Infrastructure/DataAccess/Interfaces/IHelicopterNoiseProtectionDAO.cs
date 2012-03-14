using System.Threading;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Implementations;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IHelicopterNoiseProtectionDAO : IDAO<HelicopterNoiseProtection, int>
    {
        HelicopterNoiseProtection GetByDefinitionAndCurrentCulture(HelicopterNoiseProtectionDefinition noiseProtectionDefinition);
    }

    public class HelicopterNoiseProtectionDAO : GenericDAO<HelicopterNoiseProtection,int>, IHelicopterNoiseProtectionDAO
    {
        public HelicopterNoiseProtectionDAO(ISession session) : base(session)
        {
        }

        public HelicopterNoiseProtection GetByDefinitionAndCurrentCulture(HelicopterNoiseProtectionDefinition noiseProtectionDefinition)
        {
            HelicopterNoiseProtection helicopterNoiseProtection = _session.QueryOver<HelicopterNoiseProtection>()
                .Where(x => x.HelicopterNoiseProtectionDefinition == noiseProtectionDefinition)
                .And(x => x.CultureName == Thread.CurrentThread.CurrentCulture.Name)
                .SingleOrDefault<HelicopterNoiseProtection>();

            return helicopterNoiseProtection;
        }
    }
}
