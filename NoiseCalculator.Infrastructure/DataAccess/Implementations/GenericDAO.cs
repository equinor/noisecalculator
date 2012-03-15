using System;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using NHibernate.Criterion;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.Infrastructure.DataAccess.Implementations
{
    public class GenericDAO<TEntity,TId> : IDAO<TEntity,TId> where TEntity : class
    {
        protected ISession _session;

        public GenericDAO(ISession session)
        {
            _session = session;
        }

        public virtual TEntity Get(TId id)
        {
            TEntity entity = _session.Get<TEntity>(id);
            return entity;
        }

        public TEntity GetFilteredByCurrentCulture(TId id)
        {
            TEntity entity = _session.CreateCriteria<TEntity>()
                                .Add(Restrictions.Eq("Id", id))
                                .Add(Restrictions.Eq("CultureName", Thread.CurrentThread.CurrentCulture.Name))
                                .UniqueResult<TEntity>();
                
            return entity;
        }

        public TEntity Load(TId id)
        {
            TEntity entity = _session.Load<TEntity>(id);
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> entities = _session.QueryOver<TEntity>().List<TEntity>();
            return entities;
        }

        public IEnumerable<TEntity> GetAllFilteredByCurrentCulture()
        {
            IEnumerable<TEntity> entities = _session.CreateCriteria<TEntity>()
                                                .Add(Restrictions.Eq("CultureName", Thread.CurrentThread.CurrentCulture.Name))
                                                .List<TEntity>();
            return entities;
        }

        public void Store(TEntity entity)
        {
            ITransaction transaction = null;

            try
            {
                transaction = _session.BeginTransaction();
                _session.SaveOrUpdate(entity);
                transaction.Commit();
            }
            catch (Exception)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }


        public void Delete(TEntity entity)
        {
            ITransaction transaction = null;

            try
            {
                transaction = _session.BeginTransaction();
                _session.Delete(entity);
                transaction.Commit();
            }
            catch (Exception)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }
    }
}
