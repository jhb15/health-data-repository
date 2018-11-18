using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace health_data_repository.Models
{
    public class ActivityType
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        [MinLength(1)]
        public virtual ICollection<ActivityTypeSource> Sources { get; set; }
    }
}
