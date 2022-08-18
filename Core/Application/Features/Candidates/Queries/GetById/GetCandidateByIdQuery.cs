using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Queries.GetById
{
    public class GetCandidateByIdQuery : IRequest<CandidateViewModel>
    {
        public string Id { get; set; }
    }

    internal class GetCandidateByIdQueryHandler : IRequestHandler<GetCandidateByIdQuery, CandidateViewModel>
    {
        private readonly ILogger<GetCandidateByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly Nest.ElasticClient _nestClient;

        public GetCandidateByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCandidateByIdQueryHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _nestClient = nestClient;
        }

        public async Task<CandidateViewModel> Handle(GetCandidateByIdQuery query, CancellationToken cancellationToken)
        {

            var getResponse = await _nestClient.GetAsync<Candidate>(query.Id, g => g
                .Index(EnumElasticIndexes.Candidates.ToString())
            );


            var candidate = getResponse.Source;
            if (candidate == null) throw new ApiException($"Candidate with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Candidate with id: {query.Id}");
            
            /*
            var candidate = await _repository.Candidate.GetByIdAsync(query.Id);
            if (candidate == null) throw new ApiException($"Candidate with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Candidate with id: {query.Id}");
            */
            return _mapper.Map<CandidateViewModel>(candidate);
        }
    }
}
