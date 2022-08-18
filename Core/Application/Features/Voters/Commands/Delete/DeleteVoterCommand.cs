using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.Delete
{
    public class DeleteVoterCommand : IRequest
    {
        public string Id { get; set; }
    }

    internal class DeleteVoterByIdCommandHandler : IRequestHandler<DeleteVoterCommand>
    {
        private readonly ILogger<DeleteVoterByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;

        public DeleteVoterByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteVoterByIdCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteVoterCommand command, CancellationToken cancellationToken)
        {
            var voter = await _repository.Voter.GetByIdAsync(command.Id);
            if (voter == null) throw new ApiException($"Voter with id: {command.Id}, hasn't been found.");
            await _repository.Voter.DeleteAsync(voter);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
