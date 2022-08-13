using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Account.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<JObject>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, JObject>
    {
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public ResetPasswordCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ResetPasswordCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<JObject> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(command);
            _logger.LogInformation($"Reset Password attempt with email: {command.Email} and resetToken: {command.Token}");

            var resetPasswordMessage = await _repository.Account.ResetPassword(resetPasswordRequest);
            _logger.LogInformation($"Reset Password succeeds");

            var successJson = new JObject
            {
                ["StatusCode"] = StatusCodes.Status201Created,
                ["Message"] = resetPasswordMessage
            };

            return successJson;
        }
    }
}
