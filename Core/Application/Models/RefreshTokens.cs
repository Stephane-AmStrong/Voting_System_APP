using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record RefreshTokens
    {
        public AccessToken AccessToken { get; set; }
        public UserToken RefreshToken { get; set; }
    }
}