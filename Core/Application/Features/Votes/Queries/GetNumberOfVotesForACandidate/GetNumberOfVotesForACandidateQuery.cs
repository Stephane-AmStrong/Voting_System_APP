using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Votes.Queries.GetNumberOfVotesForACandidate
{
    public class GetNumberOfVotesForACandidateQuery : IRequest<int>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid CandidateId { get; set; }
    }

    internal class GetVotesQueryHandler : IRequestHandler<GetNumberOfVotesForACandidateQuery, int>
    {
        private readonly ILogger<GetVotesQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetVotesQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetVotesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(GetNumberOfVotesForACandidateQuery query, CancellationToken cancellationToken)
        {
            var candidate = await _repository.Candidate.GetByIdAsync(query.CandidateId);
            if (candidate == null) throw new ApiException($"Candidate with id: {query.CandidateId}, hasn't been found.");

            var numberOfVotes = await _repository.Vote.GetNumberOfVoteOfCandidateByIdAsync(query.CandidateId);

            _logger.LogInformation($"Returned Number of vote of candidate with id: {query.CandidateId}");

            return numberOfVotes;
        }
    }
}
