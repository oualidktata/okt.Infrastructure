using System.Collections.Generic;
using Infra.Common.Models;
using Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.EntityFramework.Repositories
{
    public class EfWriteRepository<TEntity, TDbContext> : IWriteRepository<TEntity>
        where TEntity : BaseEntity, IRepoQueryable
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        public EfWriteRepository(
            IDbContextFactory<TDbContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();
        }

        public virtual TEntity Create(
            TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Add(entity);
            _context.SaveChanges();

            return result.Entity;
        }

        public void Create(
            IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>()
                .AddRange(entities);
            _context.SaveChanges();
        }

        public TEntity Update(
            TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Update(entity);
            _context.SaveChanges();

            return result.Entity;
        }

        public void Update(
            IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>()
                .UpdateRange(entities);
            _context.SaveChanges();
        }

        public void Delete(
            TEntity entity)
        {
            _context.Set<TEntity>()
                .Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(
            IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>()
                .RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}
