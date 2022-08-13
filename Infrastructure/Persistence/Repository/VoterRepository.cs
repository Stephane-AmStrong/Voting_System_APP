using Application.Features.Voters.Queries.GetPagedList;
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
    public class VoterRepository : RepositoryBase<Voter>, IVoterRepository
    {
        private ISortHelper<Voter> _sortHelper;

        public VoterRepository
        (
            AppDbContext appDbContext,
            ISortHelper<Voter> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Voter>> GetPagedListAsync(GetVotersQuery votersQuery)
        {
            var voters = Enumerable.Empty<Voter>().AsQueryable();

            ApplyFilters(ref voters, votersQuery);

            PerformSearch(ref voters, votersQuery.SearchTerm);

            var sortedVoters = _sortHelper.ApplySort(voters, votersQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Voter>.ToPagedList
                (
                    sortedVoters,
                    votersQuery.PageNumber,
                    votersQuery.PageSize)
                );
        }


        public async Task<Voter> GetByIdAsync(string id)
        {
            return await BaseFindByCondition(voter => voter.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(Voter voter)
        {
            return await BaseFindByCondition(x => x.FirstName == voter.FirstName && x.LastName == voter.LastName)
                .AnyAsync();
        }

        public async Task CreateAsync(Voter voter)
        {
            await BaseCreateAsync(voter);
        }

        public async Task UpdateAsync(Voter voter)
        {
            await BaseUpdateAsync(voter);
        }

        public async Task UpdateAsync(IEnumerable<Voter> voters)
        {
            await BaseUpdateAsync(voters);
        }

        public async Task DeleteAsync(Voter voter)
        {
            await BaseDeleteAsync(voter);
        }

        private void ApplyFilters(ref IQueryable<Voter> voters, GetVotersQuery votersQuery)
        {
            voters = BaseFindAll()
                .Include(x => x.Votes);

            /*
            if (votersQuery.MinCreateAt != null)
            {
                voters = voters.Where(x => x.CreateAt >= votersQuery.MinCreateAt);
            }

            if (votersQuery.MaxCreateAt != null)
            {
                voters = voters.Where(x => x.CreateAt < votersQuery.MaxCreateAt);
            }
            */
        }

        private static void PerformSearch(ref IQueryable<Voter> voters, string searchTerm)
        {
            if (!voters.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            voters = voters.Where(x => x.FirstName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) || x.LastName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }


    }
}
