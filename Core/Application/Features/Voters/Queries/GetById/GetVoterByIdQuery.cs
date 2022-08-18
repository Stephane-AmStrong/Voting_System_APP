using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Queries.GetById
{
    public class GetVoterByIdQuery : IRequest<VoterViewModel>
    {
        public string Id { get; set; }
    }

    internal class GetVoterByIdQueryHandler : IRequestHandler<GetVoterByIdQuery, VoterViewModel>
    {
        private readonly ILogger<GetVoterByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;
        private readonly IMapper _mapper;

        public GetVoterByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetVoterByIdQueryHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VoterViewModel> Handle(GetVoterByIdQuery query, CancellationToken cancellationToken)
        {
            var voter = await _repository.Voter.GetByIdAsync(query.Id);
            if (voter == null) throw new ApiException($"Voter with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Voter with id: {query.Id}");
            return _mapper.Map<VoterViewModel>(voter);
        }
    }
}
