using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Commands.RefreshAccessToken
{
    record class RefreshTokensViewModel
    {
        public AccessToken AccessToken { get; set; }
        public UserTokenViewModel RefreshToken { get; set; }
    }
}
