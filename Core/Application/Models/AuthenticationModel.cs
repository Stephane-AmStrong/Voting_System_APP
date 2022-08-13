using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record AuthenticationModel
    {
        public virtual Voter Voter { get; set; }
        public AccessToken AccessToken { get; set; }
        public UserToken RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> ErrorDetails { get; set; }
        //public DateTime? ExpireDate { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
