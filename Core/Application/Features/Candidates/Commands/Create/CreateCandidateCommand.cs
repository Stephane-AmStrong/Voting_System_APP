using Application.Features.Candidates.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.Create
{
    public class CreateCandidateCommand : IRequest<CandidateViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid CategoryId { get; set; }
    }

    internal class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CandidateViewModel>
    {
        private readonly ILogger<CreateCandidateCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateCandidateCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateCandidateCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<CandidateViewModel> Handle(CreateCandidateCommand command, CancellationToken cancellationToken)
        {
            var candidateEntity = _mapper.Map<Candidate>(command);

            await _repository.Candidate.CreateAsync(candidateEntity);
            await _repository.SaveAsync();

            return _mapper.Map<CandidateViewModel>(candidateEntity);
        }
    }
}
