using Application.Features.Votes.Queries.GetById;
using Application.Features.Votes.Queries.GetPagedList;
using Domain.Common;
using System;

namespace Application.Features.Voters.Queries.GetById
{
    public record VoterViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public virtual VotesViewModel[] Votes { get; set; }
    }
}
