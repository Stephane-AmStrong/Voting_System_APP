using Application.Interfaces;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext ApplicationDbContext { get; set; }

        public RepositoryBase(AppDbContext appDbContext)
        {
            ApplicationDbContext = appDbContext;
        }

        public IQueryable<T> BaseFindAll()
        {
            return ApplicationDbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
        {
            return ApplicationDbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task BaseCreateAsync(T entity)
        {
            await ApplicationDbContext.Set<T>().AddAsync(entity);
        }

        public async Task BaseCreateAsync(IEnumerable<T> entities)
        {
            await ApplicationDbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task BaseUpdateAsync(T entity)
        {
            await Task.Run(() => ApplicationDbContext.Set<T>().Update(entity));
        }

        public async Task BaseUpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => ApplicationDbContext.Set<T>().UpdateRange(entities));
        }

        public async Task BaseDeleteAsync(T entity)
        {
            await Task.Run(() => ApplicationDbContext.Set<T>().Remove(entity));
        }
    }
}
