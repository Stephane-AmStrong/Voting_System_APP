using Application.Enums;
using Application.Exceptions;
using Application.Features.Candidates.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.Update
{
    public class UpdateCandidateCommand : IRequest<CandidateViewModel>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CategoryId { get; set; }
    }

    internal class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, CandidateViewModel>
    {
        private readonly ILogger<UpdateCandidateCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly Nest.ElasticClient _nestClient;

        public UpdateCandidateCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateCandidateCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CandidateViewModel> Handle(UpdateCandidateCommand command, CancellationToken cancellationToken)
        {
            var candidateEntity = await _repository.Candidate.GetByIdAsync(command.Id);
            if (candidateEntity == null) throw new ApiException($"Candidate with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, candidateEntity);
            await _repository.Candidate.UpdateAsync(candidateEntity);


            var updateResponse = await _nestClient.UpdateAsync<Candidate>(candidateEntity.Id, u => u
                .Index(EnumElasticIndexes.Candidates.ToString())
                .Doc(candidateEntity)
            );


            await _repository.SaveAsync();

            var candidateReadDto = _mapper.Map<CandidateViewModel>(candidateEntity);
            return candidateReadDto;
        }
    }
}
