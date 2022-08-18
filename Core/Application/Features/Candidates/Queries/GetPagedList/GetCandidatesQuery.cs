using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Queries.GetPagedList
{
    public class GetCandidatesQuery : QueryParameters, IRequest<PagedListResponse<CandidatesViewModel>>
    {
        public GetCandidatesQuery()
        {
            OrderBy = "lastname";
        }

        public string WithFirstName { get; set; }
        public string WithLastName { get; set; }
    }

    internal class GetCandidatesQueryHandler : IRequestHandler<GetCandidatesQuery, PagedListResponse<CandidatesViewModel>>
    {
        private readonly ILogger<GetCandidatesQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly Nest.ElasticClient _nestClient;

        public GetCandidatesQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCandidatesQueryHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<CandidatesViewModel>> Handle(GetCandidatesQuery query, CancellationToken cancellationToken)
        {
            var candidates = await _repository.Candidate.GetPagedListAsync(query);
            var candidatesViewModel = _mapper.Map<List<CandidatesViewModel>>(candidates);
            _logger.LogInformation($"Returned Paged List of Candidates from database.");
            return new PagedListResponse<CandidatesViewModel>(candidatesViewModel, candidates.MetaData);
        }
    }
}
