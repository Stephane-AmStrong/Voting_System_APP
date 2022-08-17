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

namespace Application.Features.Voters.Commands.ChangeAge
{
    public class ChangeAgeVoterCommand : IRequest<VoterViewModel>
    {
        [JsonIgnore]
        public string Id { get; set; }
        public DateTime Birthday { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }
    }

    internal class ChangeAgeVoterCommandHandler : IRequestHandler<ChangeAgeVoterCommand, VoterViewModel>
    {
        private readonly ILogger<ChangeAgeVoterCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ChangeAgeVoterCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ChangeAgeVoterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VoterViewModel> Handle(ChangeAgeVoterCommand command, CancellationToken cancellationToken)
        {
            var voterEntity = await _repository.Voter.GetByIdAsync(command.Id);
            if (voterEntity == null) throw new ApiException($"Voter with id: {command.Id}, hasn't been found.");

            voterEntity.Birthday = command.Birthday;
            await _repository.Voter.UpdateAsync(voterEntity);
            await _repository.SaveAsync();

            var voterReadDto = _mapper.Map<VoterViewModel>(voterEntity);
            //if (!string.IsNullOrWhiteSpace(voterReadDto.ImgLink)) voterReadDto.ImgLink = $"{_baseURL}{voterReadDto.ImgLink}";
            return voterReadDto;
        }
    }
}
