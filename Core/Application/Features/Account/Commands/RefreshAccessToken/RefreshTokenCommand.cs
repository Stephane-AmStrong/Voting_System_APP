using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Models;
using Application.Features.Account.Commands.Authenticate;

namespace Application.Features.Account.Commands.RefreshAccessToken
{
    public class RefreshTokensCommand : IRequest<RefreshTokensViewModel>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string IpAddress { get; set; }
    }


    internal class RefreshTokenRequestCommandHandler : IRequestHandler<RefreshTokensCommand, RefreshTokensViewModel>
    {
        private readonly ILogger<RefreshTokenRequestCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RefreshTokenRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RefreshTokenRequestCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<RefreshTokensViewModel> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"RefreshToken attempt with accessToken: {command.AccessToken}");
            //var generateRefreshTokenModel = _mapper.Map<GenerateRefreshTokenModel>(command);
            var refreshTokens = await _repository.Token.RefreshAsync(command.AccessToken, command.RefreshToken, command.IpAddress);
            _logger.LogInformation($"RefreshToken succeeds");

            var refreshTokensViewModel = _mapper.Map<RefreshTokensViewModel>(refreshTokens);
            return refreshTokensViewModel;
        }
    }
}
