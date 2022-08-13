using Application.Features.Categories.Queries.GetPagedList;
using Application.Features.Voters.Queries.GetPagedList;
using Domain.Common;
using System;

namespace Application.Features.Votes.Queries.GetPagedList
{
    public record VotesViewModel : AuditableBaseEntity
    {
        public Guid VoterId { get; set; }
        public Guid CategoryId { get; set; }
        public virtual VotersViewModel Voter { get; set; }
        public virtual CategoriesViewModel Category { get; set; }
    }
}
