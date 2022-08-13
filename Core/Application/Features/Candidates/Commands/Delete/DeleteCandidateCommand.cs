using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.Delete
{
    public class DeleteCandidateCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteCandidateByIdCommandHandler : IRequestHandler<DeleteCandidateCommand>
    {
        private readonly ILogger<DeleteCandidateByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteCandidateByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteCandidateByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            var candidate = await _repository.Candidate.GetByIdAsync(command.Id);
            if (candidate == null) throw new ApiException($"Candidate with id: {command.Id}, hasn't been found.");
            await _repository.Candidate.DeleteAsync(candidate);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
