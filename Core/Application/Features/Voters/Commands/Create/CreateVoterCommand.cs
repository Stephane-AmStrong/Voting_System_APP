﻿using Application.Features.Voters.Queries.GetById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.Create
{
    public class CreateVoterCommand : IRequest<JObject>
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

    internal class CreateVoterCommandHandler : IRequestHandler<CreateVoterCommand, JObject>
    {
        private readonly ILogger<CreateVoterCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;
        private readonly IMapper _mapper;


        public CreateVoterCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateVoterCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<JObject> Handle(CreateVoterCommand command, CancellationToken cancellationToken)
        {
            var appVoter = _mapper.Map<Voter>(command);
            _logger.LogInformation($"Registration attempt with email: {command.Email}");

            var authenticationModel = await _repository.Account.RegisterAsync(appVoter, command.Password, command.Origin);
            _logger.LogInformation($"Registration succeeds");

            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
            var message = new Message(new string[] { command.Email }, "Confirm Registration", $"Please confirm your account by visiting this URL {authenticationModel.AccessToken.Value}", null);
            await _repository.Email.SendAsync(message);
            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");

            var successJson = new JObject
            {
                ["StatusCode"] = StatusCodes.Status201Created,
                ["Message"] = $"Thanks {authenticationModel.Voter.FirstName} {authenticationModel.Voter.LastName}! Your registration was successful. Check your email for verification action."
            };

            return successJson;
        }
    }
}
