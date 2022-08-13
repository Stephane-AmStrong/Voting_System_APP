using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> BaseFindAll();
        IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression);
        Task BaseCreateAsync(T entity);
        Task BaseUpdateAsync(T entity);
        Task BaseDeleteAsync(T entity);
    }
}
