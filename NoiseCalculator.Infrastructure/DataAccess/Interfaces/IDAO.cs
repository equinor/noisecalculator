using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IDAO<TEntity, in TId> where TEntity : class
    {
        TEntity Get(TId id);
        TEntity Load(TId id);
        IEnumerable<TEntity> GetAll();
        void Store(TEntity entity);
        void Delete(TEntity entity);
    }
}
