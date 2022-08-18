using Application.Features.Candidates.Queries.GetPagedList;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class CandidateRepository : RepositoryBase<Candidate>, ICandidateRepository
    {
        private ISortHelper<Candidate> _sortHelper;

        public CandidateRepository
        (
            AppDbContext appDbContext,
            ISortHelper<Candidate> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Candidate>> GetPagedListAsync(GetCandidatesQuery candidatesQuery)
        {
            var candidates = Enumerable.Empty<Candidate>().AsQueryable();

            ApplyFilters(ref candidates, candidatesQuery);

            PerformSearch(ref candidates, candidatesQuery.SearchTerm);

            var sortedCandidates = _sortHelper.ApplySort(candidates, candidatesQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Candidate>.ToPagedList
                (
                    sortedCandidates,
                    candidatesQuery.PageNumber,
                    candidatesQuery.PageSize)
                );
        }


        public async Task<Candidate> GetByIdAsync(string id)
        {
            return await BaseFindByCondition(candidate => candidate.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(Candidate candidate)
        {
            return await BaseFindByCondition(x => x.FirstName == candidate.FirstName &&  x.LastName == candidate.LastName && x.CategoryId == candidate.CategoryId)
                .AnyAsync();
        }

        public async Task CreateAsync(Candidate candidate) => await BaseCreateAsync(candidate);

        public async Task UpdateAsync(Candidate candidate) => await BaseUpdateAsync(candidate);

        public async Task UpdateAsync(IEnumerable<Candidate> candidates) => await BaseUpdateAsync(candidates);

        public async Task DeleteAsync(Candidate candidate) => await BaseDeleteAsync(candidate);

        private void ApplyFilters(ref IQueryable<Candidate> candidates, GetCandidatesQuery candidatesQuery)
        {
            candidates = BaseFindAll();

            if (!string.IsNullOrEmpty(candidatesQuery.WithFirstName))
            {
                candidates = candidates.Where(x => x.FirstName.Equals(candidatesQuery.WithFirstName));
            }

            if (!string.IsNullOrEmpty(candidatesQuery.WithLastName))
            {
                candidates = candidates.Where(x => x.LastName.Equals(candidatesQuery.WithLastName));
            }
        }

        private static void PerformSearch(ref IQueryable<Candidate> candidates, string searchTerm)
        {
            if (!candidates.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            candidates = candidates.Where(x => x.FirstName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) || x.LastName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
