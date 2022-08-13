using Application.Features.Votes.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVoteRepository
    {
        Task<PagedList<Vote>> GetPagedListAsync(GetVotesQuery getVotesQuery);

        Task<Vote> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Vote vote);

        Task CreateAsync(Vote vote);
        Task UpdateAsync(Vote vote);
        Task UpdateAsync(IEnumerable<Vote> events);
        Task DeleteAsync(Vote vote);
    }
}
