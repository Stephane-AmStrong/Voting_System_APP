using Application.Features.Categories.Queries.GetPagedList;
using Application.Features.Voters.Queries.GetPagedList;
using Domain.Common;
using System;

namespace Application.Features.Votes.Queries.GetPagedList
{
    public record VotesViewModel
    {
        public virtual Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VoterId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CandidateId { get; set; }
        public virtual VotersViewModel Voter { get; set; }
        public virtual CategoriesViewModel Category { get; set; }
    }
}
