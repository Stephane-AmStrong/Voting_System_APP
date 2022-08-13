using Application.Features.Candidates.Queries.GetPagedList;
using Application.Features.Votes.Queries.GetPagedList;
using Domain.Common;

namespace Application.Features.Categories.Queries.GetById
{
    public record CategoryViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual CandidatesViewModel[] Candidates { get; set; }
        public virtual VotesViewModel[] Votes { get; set; }
    }
}
