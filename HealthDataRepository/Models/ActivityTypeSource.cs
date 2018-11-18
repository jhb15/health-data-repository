using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public class ActivityTypeSource
    {

        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual DataSource Source { get; set; }

        [Required]
        [MinLength(1)]
        public virtual IList<SourceActivity> SourceActivities { get; set; }
    }
}
