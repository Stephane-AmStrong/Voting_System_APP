using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Account.Commands.RefreshAccessToken
{
    public class UserTokenViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
