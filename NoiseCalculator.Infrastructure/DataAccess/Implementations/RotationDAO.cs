using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class RotationDAO : GenericDAO<Rotation,int>,IRotationDAO
    {
        public RotationDAO(ISession session) : base(session)
        {
        }

        public Rotation GetByTaskId(int taskId)
        {
            return _session.QueryOver<Rotation>()
                .Where(x => x.Task.Id == taskId)
                .SingleOrDefault<Rotation>();
        }

        public IEnumerable<Rotation> GetAllFetchTaskOrderedByTaskTitle()
        {
            return _session.QueryOver<Rotation>()
                .Fetch(x => x.Task).Eager
                .Fetch(x => x.OperatorTask).Eager
                .Fetch(x => x.AssistantTask).Eager
                .List<Rotation>();
        }
    }
}