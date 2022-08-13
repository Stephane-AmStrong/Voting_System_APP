using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record AccessToken
    {
        public string Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
