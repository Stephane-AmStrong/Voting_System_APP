using Application.Features.Votes.Queries.GetPagedList;
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
    public class VoteRepository : RepositoryBase<Vote>, IVoteRepository
    {
        private ISortHelper<Vote> _sortHelper;

        public VoteRepository
        (
            AppDbContext appDbContext,
            ISortHelper<Vote> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Vote>> GetPagedListAsync(GetVotesQuery votesQuery)
        {
            var votes = Enumerable.Empty<Vote>().AsQueryable();

            ApplyFilters(ref votes, votesQuery);

            PerformSearch(ref votes, votesQuery.SearchTerm);

            var sortedVotes = _sortHelper.ApplySort(votes, votesQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Vote>.ToPagedList
                (
                    sortedVotes,
                    votesQuery.PageNumber,
                    votesQuery.PageSize)
                );
        }


        public async Task<Vote> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(vote => vote.Id.Equals(id))
                .Include(x => x.Category)
                .Include(x => x.Voter)
                .FirstOrDefaultAsync();
        }


        public async Task<int> GetNumberOfVoteOfCandidateByIdAsync(Guid candidateId)
        {
            return await BaseFindByCondition(vote => vote.CandidateId == candidateId).CountAsync();
        }


        public async Task<bool> ExistAsync(Vote vote)
        {
            return await BaseFindByCondition(x => x.CategoryId == vote.CategoryId && x.VoterId == vote.VoterId)
                .AnyAsync();
        }

        public async Task CreateAsync(Vote vote)
        {
            await BaseCreateAsync(vote);
        }

        public async Task UpdateAsync(Vote vote)
        {
            await BaseUpdateAsync(vote);
        }

        public async Task UpdateAsync(IEnumerable<Vote> votes)
        {
            await BaseUpdateAsync(votes);
        }

        public async Task DeleteAsync(Vote vote)
        {
            await BaseDeleteAsync(vote);
        }

        private void ApplyFilters(ref IQueryable<Vote> votes, GetVotesQuery votesQuery)
        {
            votes = BaseFindAll()
                .Include(x => x.Category)
                .Include(x => x.Voter);

            /*
            if (votesQuery.MinCreateAt != null)
            {
                votes = votes.Where(x => x.CreateAt >= votesQuery.MinCreateAt);
            }

            if (votesQuery.MaxCreateAt != null)
            {
                votes = votes.Where(x => x.CreateAt < votesQuery.MaxCreateAt);
            }
            */
        }

        private static void PerformSearch(ref IQueryable<Vote> votes, string searchTerm)
        {
            if (!votes.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            votes = votes.Where(x => x.Category.Name.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) || x.Voter.FirstName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) || x.Voter.LastName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
