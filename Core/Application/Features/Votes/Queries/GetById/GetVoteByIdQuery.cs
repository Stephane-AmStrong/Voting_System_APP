using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Votes.Queries.GetById
{
    public class GetVoteByIdQuery : IRequest<VoteViewModel>
    {
        public string Id { get; set; }
    }

    internal class GetVoteByIdQueryHandler : IRequestHandler<GetVoteByIdQuery, VoteViewModel>
    {
        private readonly ILogger<GetVoteByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;
        private readonly IMapper _mapper;

        public GetVoteByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetVoteByIdQueryHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VoteViewModel> Handle(GetVoteByIdQuery query, CancellationToken cancellationToken)
        {
            var vote = await _repository.Vote.GetByIdAsync(query.Id);
            if (vote == null) throw new ApiException($"Vote with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Vote with id: {query.Id}");
            return _mapper.Map<VoteViewModel>(vote);
        }
    }
}
