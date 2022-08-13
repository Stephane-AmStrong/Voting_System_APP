using Application.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Application.Features.Account.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<JObject>
    {
        public string Email { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }
    }

    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, JObject>
    {
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;


        public ForgotPasswordCommandHandler(IRepositoryWrapper repository, ILogger<ForgotPasswordCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<JObject> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Forgot password attempt with Email: {command.Email}");
            var authenticationModel = await _repository.Token.GeneratePasswordResetTokenAsync(command.Email);
            _logger.LogInformation($"Forgot password succeeds");

            if (authenticationModel.IsSuccess)
            {
                _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
                var message = new Message(new string[] { command.Email }, "Reset Password", $"You reset token is:  {authenticationModel.AccessToken.Value}", null);
                await _repository.Email.SendAsync(message);
                _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
            }

            var successJson = new JObject
            {
                ["StatusCode"] = StatusCodes.Status201Created,
                ["Message"] = $"Password reset url has been sent to your email successfully."
            };

            return successJson;
        }
    }
}
