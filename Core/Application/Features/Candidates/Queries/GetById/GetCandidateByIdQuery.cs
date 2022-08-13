using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Queries.GetById
{
    public class GetCandidateByIdQuery : IRequest<CandidateViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetCandidateByIdQueryHandler : IRequestHandler<GetCandidateByIdQuery, CandidateViewModel>
    {
        private readonly ILogger<GetCandidateByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetCandidateByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCandidateByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CandidateViewModel> Handle(GetCandidateByIdQuery query, CancellationToken cancellationToken)
        {
            var candidate = await _repository.Candidate.GetByIdAsync(query.Id);
            if (candidate == null) throw new ApiException($"Candidate with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Candidate with id: {query.Id}");
            return _mapper.Map<CandidateViewModel>(candidate);
        }
    }
}
