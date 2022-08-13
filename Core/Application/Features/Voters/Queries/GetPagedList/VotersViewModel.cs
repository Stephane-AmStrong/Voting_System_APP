using Domain.Common;
using System;

namespace Application.Features.Voters.Queries.GetPagedList
{
    public record VotersViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
