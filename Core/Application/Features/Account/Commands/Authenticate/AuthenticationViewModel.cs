using Application.Features.Account.Commands.RefreshAccessToken;
using Application.Features.Voters.Queries.GetPagedList;
using Application.Models;

namespace Application.Features.Account.Commands.Authenticate
{
    public class AuthenticationViewModel
    {
        public VotersViewModel Voter { get; set; }
        public AccessToken AccessToken { get; set; }
        public UserTokenViewModel RefreshToken { get; set; }
    }
}
