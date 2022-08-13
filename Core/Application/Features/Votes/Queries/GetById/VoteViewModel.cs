using Application.Features.Categories.Queries.GetById;
using Application.Features.Voters.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.Votes.Queries.GetById
{
    public record VoteViewModel : AuditableBaseEntity
    {
        public Guid VoterId { get; set; }
        public Guid CategoryId { get; set; }

        public virtual VoterViewModel Voter { get; set; }
        public virtual CategoryViewModel Category { get; set; }
    }
}
