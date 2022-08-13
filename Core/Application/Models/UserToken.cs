using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record UserToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Value { get; set; }

        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsActive => (DateTime.UtcNow <= ExpiryDate);
        //public virtual AppUser AppUser { get; set; }
    }
}
