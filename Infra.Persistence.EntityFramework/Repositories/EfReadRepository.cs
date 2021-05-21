using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infra.Common.Models;
using Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.EntityFramework.Repositories
{
    public class EfReadRepository<TEntity, TDbContext> : IReadRepository<TEntity>
        where TEntity : BaseEntity, IRepoQueryable
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        public EfReadRepository(
            IDbContextFactory<TDbContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => Queryable.ElementType;

        public Expression Expression => Queryable.Expression;

        public IQueryProvider Provider => Queryable.Provider;

        protected virtual IQueryable<TEntity> Queryable => _context.Set<TEntity>()
            .AsQueryable();
    }
}
