using System.Linq;
using Infra.Common.Models;

namespace Infra.Persistence.Repositories
{
    public interface IReadRepository<TEntity> : IQueryable<TEntity> where TEntity : BaseEntity, IRepoQueryable
    {
        
    }
}
