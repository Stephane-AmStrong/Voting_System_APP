using Application.Features.Votes.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Votes.Commands.Create
{
    public class CreateVoteCommand : IRequest<VoteViewModel>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string VoterId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CandidateId { get; set; }
    }

    internal class CreateVoteCommandHandler : IRequestHandler<CreateVoteCommand, VoteViewModel>
    {
        private readonly ILogger<CreateVoteCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateVoteCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateVoteCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<VoteViewModel> Handle(CreateVoteCommand command, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Vote>(command);

            await _repository.Vote.CreateAsync(productEntity);
            await _repository.SaveAsync();

            return _mapper.Map<VoteViewModel>(productEntity);
        }
    }
}
