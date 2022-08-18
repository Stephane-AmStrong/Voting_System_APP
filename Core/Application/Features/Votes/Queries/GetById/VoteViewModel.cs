using Application.Features.Categories.Queries.GetById;
using Application.Features.Voters.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.Votes.Queries.GetById
{
    public record VoteViewModel:AuditableBaseEntity
    {
        public string VoterId { get; set; }
        public string CategoryId { get; set; }
        public string CandidateId { get; set; }

        public virtual VoterViewModel Voter { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }
}
