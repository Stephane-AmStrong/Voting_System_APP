using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.Delete
{
    public class DeleteCandidateCommand : IRequest
    {
        public string Id { get; set; }
    }

    internal class DeleteCandidateByIdCommandHandler : IRequestHandler<DeleteCandidateCommand>
    {
        private readonly ILogger<DeleteCandidateByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;

        public DeleteCandidateByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteCandidateByIdCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _logger = logger;
            _nestClient = nestClient;
        }

        public async Task<Unit> Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            var candidate = await _repository.Candidate.GetByIdAsync(command.Id);
            if (candidate == null) throw new ApiException($"Candidate with id: {command.Id}, hasn't been found.");

            var deleteResponse = await _nestClient.DeleteAsync<Candidate>(candidate.Id, d => d.Index(EnumElasticIndexes.Candidates.ToString()));

            await _repository.Candidate.DeleteAsync(candidate);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
