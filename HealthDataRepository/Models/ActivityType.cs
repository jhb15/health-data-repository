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

        [MinLength(1)]
        public virtual ICollection<ActivityMapping> Mappings { get; set; }
    }
}
