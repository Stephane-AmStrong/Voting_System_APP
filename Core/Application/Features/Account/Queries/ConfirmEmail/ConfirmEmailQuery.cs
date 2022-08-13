using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Account.Queries.ConfirmEmail
{
    public class ConfirmEmailQuery : IRequest<string>
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailQuery, string>
    {
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;


        public ConfirmEmailCommandHandler(IRepositoryWrapper repository, ILogger<ConfirmEmailCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<string> Handle(ConfirmEmailQuery command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Email confirmation attempt with UserId: {command.UserId}");
            var emailConfirmationMessage = await _repository.Account.ConfirmEmailAsync(command.UserId, command.Code);

            _logger.LogInformation($"Email confirmation succeeds");
            return emailConfirmationMessage;
        }
    }
}
