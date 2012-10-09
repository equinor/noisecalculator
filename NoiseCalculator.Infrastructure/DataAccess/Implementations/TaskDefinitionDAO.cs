using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class TaskDefinitionDAO : GenericDAO<TaskDefinition, int>, ITaskDefinitionDAO
    {
        public TaskDefinitionDAO(ISession session) : base(session)
        {
        }

        public IEnumerable<TaskDefinition> GetAllOrdered()
        {
            IEnumerable<TaskDefinition> entities = _session.QueryOver<TaskDefinition>()
                .OrderBy(x => x.SystemName).Asc
                .List<TaskDefinition>();
            
            return entities;
        }
    }
}