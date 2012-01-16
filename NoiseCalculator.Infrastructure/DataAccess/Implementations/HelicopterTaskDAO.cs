using System;
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

        public HelicopterTask Get(int helicopterTypeId, int helicopterNoiseProtectionId, int helicopterWorkIntervalId)
        {
            HelicopterTask helicopterTask = _session.QueryOver<HelicopterTask>()
                .Where(x => x.HelicopterType.Id == helicopterTypeId)
                .And(x => x.HelicopterNoiseProtection.Id == helicopterNoiseProtectionId)
                .And(x => x.HelicopterWorkInterval.Id == helicopterWorkIntervalId)
                .Fetch(x => x.HelicopterNoiseProtection).Eager
                .Fetch(x => x.HelicopterType).Eager
                .Fetch(x => x.HelicopterWorkInterval).Eager
                .SingleOrDefault<HelicopterTask>();

            return helicopterTask;
        }
    }
}