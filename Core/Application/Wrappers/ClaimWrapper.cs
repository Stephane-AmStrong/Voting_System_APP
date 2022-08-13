using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class ClaimWrapper : Claim
    {
        public ClaimWrapper(string policyName, string type, string value) : base(type, value)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; set; }
    }
}
