using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public record Candidate : AuditableBaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Guid? CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
