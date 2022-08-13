using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Application.Models;

namespace Application.Features.Account.Commands.RevokeAccessToken
{
    public class RevokeTokenCommand : IRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }
    }


    internal class RevokeTokenRequestCommandHandler: IRequestHandler<RevokeTokenCommand>
    {
        private readonly ILogger<RevokeTokenRequestCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RevokeTokenRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RevokeTokenRequestCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Unit> Handle(RevokeTokenCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retreiving user: |{command.UserId}|'s Refresh Token");
            var refreshToken = await _repository.Token.GetByUserIdAsync(command.UserId);

            _logger.LogInformation($"Attempting to delete user: |{command.UserId}|'s Refresh Token");
            await _repository.Token.DeleteAsync(refreshToken);

            await _repository.Token.SaveAsync();
            _logger.LogInformation($"Refresh Token deletion complete");

            return Unit.Value;
        }
    }
}
