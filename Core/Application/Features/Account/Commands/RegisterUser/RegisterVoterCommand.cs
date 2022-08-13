using Application.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Account.Commands.RegisterVoter
{
    public class RegisterVoterCommand : IRequest<JObject>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }

    }

    internal class RegisterVoterCommandHandler : IRequestHandler<RegisterVoterCommand, JObject>
    {
        private readonly ILogger<RegisterVoterCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RegisterVoterCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RegisterVoterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<JObject> Handle(RegisterVoterCommand command, CancellationToken cancellationToken)
        {
            var appVoter = _mapper.Map<Voter>(command);
            _logger.LogInformation($"Registration attempt with email: {command.Email}");

            var authenticationModel = await _repository.Account.RegisterAsync(appVoter, command.Password, command.Origin);
            _logger.LogInformation($"Registration succeeds");

            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
            var message = new Message(new string[] { command.Email }, "Confirm Registration", $"Please confirm your account by visiting this URL {authenticationModel.AccessToken.Value}", null);
            await _repository.Email.SendAsync(message);
            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");

            //return $"{authenticationModel.VoterInfo["name"]}, message: Voter Registered. Please check your email for verification action.";

            var successJson = new JObject
            {
                ["StatusCode"] = StatusCodes.Status201Created,
                ["Message"] = $"Thanks {authenticationModel.Voter.FirstName} {authenticationModel.Voter.LastName}! Your registration was successful. Check your email for verification action."
            };


            return successJson;
        }
    }
}
