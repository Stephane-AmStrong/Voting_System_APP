using Application.Enums;
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
        public string CategoryId { get; set; }
        public string CandidateId { get; set; }
    }

    internal class CreateVoteCommandHandler : IRequestHandler<CreateVoteCommand, VoteViewModel>
    {
        private readonly ILogger<CreateVoteCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;
        private readonly IMapper _mapper;


        public CreateVoteCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateVoteCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<VoteViewModel> Handle(CreateVoteCommand command, CancellationToken cancellationToken)
        {
            var voteEntity = _mapper.Map<Vote>(command);

            var response = await _nestClient.IndexAsync(voteEntity,
                x => x.Index(EnumElasticIndexes.Votes.ToString())
            );
            voteEntity.Id = response.Id;

            await _repository.Vote.CreateAsync(voteEntity);
            await _repository.SaveAsync();

            return _mapper.Map<VoteViewModel>(voteEntity);
        }
    }
}
