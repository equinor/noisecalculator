using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class RotationDAO : GenericDAO<Rotation,int>, IRotationDAO
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

        public IList<Rotation> GetAllByTaskDefinitionIdOrderedByTaskTitle(int taskDefinitionId)
        {
            return _session.QueryOver<Rotation>()
                .Fetch(x => x.Task).Eager
                .Fetch(x => x.OperatorTask).Eager
                .Fetch(x => x.OperatorTask.Role).Eager
                .Fetch(x => x.AssistantTask).Eager
                .Fetch(x => x.AssistantTask.Role).Eager
                .JoinQueryOver(x => x.Task)
                    .Where(x => x.TaskDefinition.Id == taskDefinitionId)
                    .OrderBy(x => x.Title).Asc
                .List<Rotation>();
        }
    }
}