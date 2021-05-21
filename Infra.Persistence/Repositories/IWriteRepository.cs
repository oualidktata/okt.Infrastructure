using System.Collections.Generic;
using Infra.Common.Models;

namespace Infra.Persistence.Repositories
{
    public interface IWriteRepository<TEntity> where TEntity : BaseEntity, IRepoQueryable
    {
        TEntity Create(TEntity entity);
        void Create(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        void Update(IEnumerable<TEntity> entity);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }
}
