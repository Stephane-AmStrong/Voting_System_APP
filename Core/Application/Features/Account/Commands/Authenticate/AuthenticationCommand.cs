using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.Features.Account.Commands.Authenticate
{
    public class AuthenticationCommand : IRequest<AuthenticationViewModel>
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string IpAddress { get; set; }
    }


    internal class AuthenticationRequestCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationViewModel>
    {
        private readonly ILogger<AuthenticationRequestCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public AuthenticationRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<AuthenticationRequestCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<AuthenticationViewModel> Handle(AuthenticationCommand command, CancellationToken cancellationToken)
        {
            var loginModel = _mapper.Map<LoginModel>(command);

            _logger.LogInformation($"Authentication attempt with email: {command.Email}");
            var authenticationModel = await _repository.Account.AuthenticateAsync(loginModel, command.IpAddress);
            _logger.LogInformation($"Authentication succeeds");

            var refreshTokenModel = await _repository.Token.CommitAsync(authenticationModel.RefreshToken);
            _logger.LogInformation($"RefreshToken succeeds");
            authenticationModel.RefreshToken = refreshTokenModel;

            var authenticationViewModel = _mapper.Map<AuthenticationViewModel>(authenticationModel);

            return authenticationViewModel;
        }
    }
}
