using Application.Features.Categories.Queries.GetById;
using Application.Features.Votes.Queries.GetById;
using Domain.Common;

namespace Application.Features.Candidates.Queries.GetById
{
    public record CandidateViewModel : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }
}
