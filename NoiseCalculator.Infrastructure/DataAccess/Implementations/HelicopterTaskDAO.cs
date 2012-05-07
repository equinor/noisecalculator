using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class HelicopterTaskDAO : GenericDAO<HelicopterTask, int>, IHelicopterTaskDAO
    {
        public HelicopterTaskDAO(ISession session) : base(session)
        {
        }

        public HelicopterTask Get(int helicopterId, HelicopterNoiseProtectionDefinition noiseProtectionDefinition, int workIntervalId)
        {
            HelicopterTask helicopterTask = _session.QueryOver<HelicopterTask>()
                .Where(x => x.HelicopterType.Id == helicopterId)
                .And(x => x.HelicopterWorkInterval.Id == workIntervalId)
                .And(x => x.HelicopterNoiseProtectionDefinition == noiseProtectionDefinition)
                .Fetch(x => x.HelicopterType).Eager
                .Fetch(x => x.HelicopterWorkInterval).Eager
                .SingleOrDefault<HelicopterTask>();

            return helicopterTask;
        }

        public HelicopterTask Get(int helicopterId, int noiseProtectionDefinitionId, int workIntervalId)
        {
            HelicopterTask helicopterTask = _session.QueryOver<HelicopterTask>()
                .Where(x => x.HelicopterType.Id == helicopterId)
                .And(x => x.HelicopterWorkInterval.Id == workIntervalId)
                .And(x => x.HelicopterNoiseProtectionDefinition.Id == noiseProtectionDefinitionId)
                .Fetch(x => x.HelicopterType).Eager
                .Fetch(x => x.HelicopterWorkInterval).Eager
                .SingleOrDefault<HelicopterTask>();

            return helicopterTask;
        }
    }
}