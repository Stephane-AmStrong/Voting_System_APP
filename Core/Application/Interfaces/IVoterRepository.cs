using Application.Features.Voters.Queries.GetPagedList;
using Application.Models;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVoterRepository
    {
        Task<PagedList<Voter>> GetPagedListAsync(GetVotersQuery getAllVotersQuery);

        Task<Voter> GetByIdAsync(string id);
        Task<bool> ExistAsync(Voter voter);

        Task CreateAsync(Voter voter);
        Task UpdateAsync(Voter voter);
        Task UpdateAsync(IEnumerable<Voter> events);
        Task DeleteAsync(Voter voter);
    }
}
