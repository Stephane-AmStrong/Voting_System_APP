using Domain.Common;
using System;

namespace Application.Features.Candidates.Queries.GetPagedList
{
    public record CandidatesViewModel : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
