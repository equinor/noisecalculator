using System.Collections.Generic;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IDAO<TEntity, in TId> where TEntity : class
    {
        TEntity Get(TId id);
        TEntity GetFilteredByCurrentCulture(TId id);
        TEntity Load(TId id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllFilteredByCurrentCulture();
        void Store(TEntity entity);
        void Delete(TEntity entity);
    }
}
