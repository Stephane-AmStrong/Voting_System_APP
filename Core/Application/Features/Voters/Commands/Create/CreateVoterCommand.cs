using Application.Features.Voters.Queries.GetById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.Create
{
    public class CreateVoterCommand : IRequest<VoterViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }
    }

    internal class CreateVoterCommandHandler : IRequestHandler<CreateVoterCommand, VoterViewModel>
    {
        private readonly ILogger<CreateVoterCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateVoterCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateVoterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<VoterViewModel> Handle(CreateVoterCommand command, CancellationToken cancellationToken)
        {
            var voterEntity = _mapper.Map<Voter>(command);

            await _repository.Voter.CreateAsync(voterEntity);
            await _repository.SaveAsync();

            return _mapper.Map<VoterViewModel>(voterEntity);
        }
    }
}
