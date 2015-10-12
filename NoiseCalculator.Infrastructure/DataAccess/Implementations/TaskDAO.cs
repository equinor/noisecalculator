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
                .Fetch(x => x.TaskDefinition).Eager
                .OrderBy(x => x.SortOrder).Desc
                .ThenBy(x => x.Title).Asc
                .List<Task>();
            
            return entities;
        }

        public IEnumerable<Task> GetAllByTaskDefinitionIdOrdered(int id)
        {
            IEnumerable<Task> entities = _session.QueryOver<Task>()
                .Where(x => x.CultureName == Thread.CurrentThread.CurrentCulture.Name)
                .And(x => x.TaskDefinition.Id == id)
                .Fetch(x => x.Role).Eager
                .OrderBy(x => x.SortOrder).Asc
                .ThenBy(x => x.Title).Asc
                .List<Task>();

            return entities;
        }

        public IEnumerable<Task> GetAllHelideckByTaskDefinitionIdOrdered(int id)
        {
            IEnumerable<Task> entities = _session.QueryOver<Task>()
                .Where(x => x.TaskDefinition.Id == id)
                .Fetch(x => x.Role).Eager
                .OrderBy(x => x.SortOrder).Desc
                .ThenBy(x => x.Title).Asc
                .List<Task>();

            return entities;
        }

    }
}