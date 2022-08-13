using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class IdentityRepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext AppDbContext { get; set; }


        public IdentityRepositoryBase(AppDbContext identityContext)
        {
            AppDbContext = identityContext;
        }

        public IQueryable<T> BaseFindAll()
        {
            return AppDbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
        {
            return AppDbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task BaseCreateAsync(T entity)
        {
            await AppDbContext.Set<T>().AddAsync(entity);
        }

        public async Task BaseCreateAsync(IEnumerable<T> entities)
        {
            await AppDbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task BaseUpdateAsync(T entity)
        {
            await Task.Run(() => AppDbContext.Set<T>().Update(entity));
        }

        public async Task BaseUpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => AppDbContext.Set<T>().UpdateRange(entities));
        }

        public async Task BaseDeleteAsync(T entity)
        {
            await Task.Run(() => AppDbContext.Set<T>().Remove(entity));
        }

        public async Task BaseSaveAsync()
        {
            await AppDbContext.SaveChangesAsync();
        }
    }
}
