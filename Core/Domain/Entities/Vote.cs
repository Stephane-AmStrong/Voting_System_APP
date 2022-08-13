using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Vote : AuditableBaseEntity
    {
        [Required]
        public string VoterId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }



        [ForeignKey("VoterId")]
        public virtual Voter Voter { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
