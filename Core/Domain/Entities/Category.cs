using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Category : AuditableBaseEntity
    {
        public Category()
        {
            Candidates = new HashSet<Candidate>();
            Votes = new HashSet<Vote>();
        }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
