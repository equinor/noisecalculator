using System;
using System.Collections.Generic;
using NHibernate;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class SelectedTaskDAO : GenericDAO<SelectedTask,int>, ISelectedTaskDAO
    {
        public SelectedTaskDAO(ISession session) : base(session)
        {
        }

        public IEnumerable<SelectedTask> GetAllChronologically(string createdByUsername, DateTime createdDate)
        {
            IEnumerable<SelectedTask> selectedTasks = _session.QueryOver<SelectedTask>()
                .Where(x => x.CreatedBy == createdByUsername)
                .And(x => x.CreatedDate == createdDate.Date)
                .Fetch(x => x.Role).Eager
                .OrderBy(x => x.Id).Asc
                .JoinQueryOver(x => x.Task)
                .List<SelectedTask>();

            return selectedTasks;
        }
    }
}