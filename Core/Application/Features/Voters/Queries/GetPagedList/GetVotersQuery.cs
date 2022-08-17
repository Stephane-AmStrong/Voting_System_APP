using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Queries.GetPagedList
{
    public class GetVotersQuery : QueryParameters, IRequest<PagedListResponse<VotersViewModel>>
    {
        public GetVotersQuery()
        {
            OrderBy = "lastname";
        }

        public string WithFirstName { get; set; }
        public string WithLastName { get; set; }
    }

    internal class GetVotersQueryHandler : IRequestHandler<GetVotersQuery, PagedListResponse<VotersViewModel>>
    {
        private readonly ILogger<GetVotersQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetVotersQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetVotersQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<VotersViewModel>> Handle(GetVotersQuery query, CancellationToken cancellationToken)
        {
            var voters = await _repository.Voter.GetPagedListAsync(query);
            var votersViewModel = _mapper.Map<List<VotersViewModel>>(voters);
            _logger.LogInformation($"Returned Paged List of Voters from database.");
            return new PagedListResponse<VotersViewModel>(votersViewModel, voters.MetaData);
        }
    }
}
