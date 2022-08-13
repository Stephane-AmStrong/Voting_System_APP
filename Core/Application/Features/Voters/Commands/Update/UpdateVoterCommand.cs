using Application.Exceptions;
using Application.Features.Voters.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.Update
{
    public class UpdateVoterCommand : IRequest<VoterViewModel>
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }
    }

    internal class UpdateVoterCommandHandler : IRequestHandler<UpdateVoterCommand, VoterViewModel>
    {
        private readonly ILogger<UpdateVoterCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateVoterCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateVoterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VoterViewModel> Handle(UpdateVoterCommand command, CancellationToken cancellationToken)
        {
            var voterEntity = await _repository.Voter.GetByIdAsync(command.Id);
            if (voterEntity == null) throw new ApiException($"Voter with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, voterEntity);
            await _repository.Voter.UpdateAsync(voterEntity);
            await _repository.SaveAsync();

            var voterReadDto = _mapper.Map<VoterViewModel>(voterEntity);
            //if (!string.IsNullOrWhiteSpace(voterReadDto.ImgLink)) voterReadDto.ImgLink = $"{_baseURL}{voterReadDto.ImgLink}";
            return voterReadDto;
        }
    }
}
