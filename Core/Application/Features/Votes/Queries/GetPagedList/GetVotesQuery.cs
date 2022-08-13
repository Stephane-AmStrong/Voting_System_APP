using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Votes.Queries.GetPagedList
{
    public class GetVotesQuery : QueryParameters, IRequest<PagedListResponse<VotesViewModel>>
    {
        public GetVotesQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetVotesQueryHandler : IRequestHandler<GetVotesQuery, PagedListResponse<VotesViewModel>>
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

        public async Task<PagedListResponse<VotesViewModel>> Handle(GetVotesQuery query, CancellationToken cancellationToken)
        {
            var votes = await _repository.Vote.GetPagedListAsync(query);
            var votesViewModel = _mapper.Map<List<VotesViewModel>>(votes);
            _logger.LogInformation($"Returned Paged List of Votes from database.");
            return new PagedListResponse<VotesViewModel>(votesViewModel, votes.MetaData);
        }
    }
}
