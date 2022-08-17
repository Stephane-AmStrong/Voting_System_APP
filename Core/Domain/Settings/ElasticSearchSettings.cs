using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Settings
{
    public record ElasticSearchSettings
    {
        public string Uri { get; set; }
    }
}
