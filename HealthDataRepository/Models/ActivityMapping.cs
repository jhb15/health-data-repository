using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public class ActivityMapping
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Source { get; set; }

        [Required]
        public virtual string MappingKey { get; set; }

        [Required]
        public virtual int ActivityTypeId { get; set; }

    }
}
