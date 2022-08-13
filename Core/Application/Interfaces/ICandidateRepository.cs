using Application.Features.Candidates.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICandidateRepository
    {
        Task<PagedList<Candidate>> GetPagedListAsync(GetCandidatesQuery getCandidatesQuery);

        Task<Candidate> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Candidate candidate);

        Task CreateAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task UpdateAsync(IEnumerable<Candidate> events);
        Task DeleteAsync(Candidate candidate);
    }
}
