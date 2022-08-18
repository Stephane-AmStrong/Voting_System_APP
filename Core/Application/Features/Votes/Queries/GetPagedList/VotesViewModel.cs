using Application.Features.Categories.Queries.GetPagedList;
using Application.Features.Voters.Queries.GetPagedList;
using Domain.Common;
using System;

namespace Application.Features.Votes.Queries.GetPagedList
{
    public record VotesViewModel
    {
        public virtual string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VoterId { get; set; }
        public string CategoryId { get; set; }
        public string CandidateId { get; set; }
        public virtual VotersViewModel Voter { get; set; }
        public virtual CategoriesViewModel Category { get; set; }
    }
}
