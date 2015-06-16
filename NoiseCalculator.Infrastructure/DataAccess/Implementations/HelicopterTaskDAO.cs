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

        public HelicopterTask Get(int helicopterId, int taskId)
        {
            HelicopterTask helicopterTask = _session.QueryOver<HelicopterTask>()
                .Where(x => x.HelicopterType.Id == helicopterId)
                .And(x => x.Task.Id == taskId)
                .Fetch(x => x.HelicopterType).Eager
                .Fetch(x => x.NoiseLevel).Eager
                .Fetch(x=> x.Task).Eager
                .SingleOrDefault<HelicopterTask>();

            return helicopterTask;
        }

        public IEnumerable<HelicopterTask> GetAllOrderedByType()
        {
            const string query = @"
                SELECT
                    task
                FROM
                    HelicopterTask as task
                    JOIN task.HelicopterType as type
                ORDER BY
                    type.Title asc";

            var helicopterTasks = _session.CreateQuery(query).List<HelicopterTask>();

            return helicopterTasks;
        }
    }
}
