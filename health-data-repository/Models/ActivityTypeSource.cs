using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace health_data_repository.Models
{
    public class ActivityTypeSource
    {
        [Required]
        public virtual DataSource Source { get; set; }

        [Required]
        [MinLength(1)]
        public virtual List<string> SourceActivityIds { get; set; }
    }
}
