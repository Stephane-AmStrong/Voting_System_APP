using Application.Features.Categories.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedList<Category>> GetPagedListAsync(GetCategoriesQuery getCategoriesQuery);

        Task<Category> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Category category);

        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task UpdateAsync(IEnumerable<Category> categories);
        Task DeleteAsync(Category category);
    }
}
