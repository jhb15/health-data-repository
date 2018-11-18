using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public class ActivityType
    {

        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        [MinLength(1)]
        public virtual ICollection<ActivityTypeSource> Sources { get; set; }
    }
}
