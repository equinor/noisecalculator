using System.Collections.Generic;
using System.Threading;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class TaskDAO : GenericDAO<Task, int>, ITaskDAO
    {
        public TaskDAO(ISession session) : base(session)
        {
        }

        public IEnumerable<Task> GetAllOrdered()
        {
            IEnumerable<Task> entities = _session.QueryOver<Task>()
                .Where(x => x.CultureName == Thread.CurrentThread.CurrentCulture.Name)
                .Fetch(x => x.Role).Eager
                .OrderBy(x => x.SortOrder).Desc
                .ThenBy(x => x.Title).Asc
                .List<Task>();
            
            return entities;
        }

        public IEnumerable<Task> GetAllByTaskDefinitionOrdered(string systemName)
        {
            IEnumerable<Task> entities = _session.QueryOver<Task>()
                .Where(x => x.CultureName == Thread.CurrentThread.CurrentCulture.Name)
                .And(x=> x.TaskDefinition.SystemName == systemName)
                .Fetch(x => x.Role).Eager
                .OrderBy(x => x.SortOrder).Desc
                .ThenBy(x => x.Title).Asc
                .List<Task>();

            return entities;
        }
    }
}