using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public class EmailRecord
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string UserId { get; set; }

        [Required]
        public virtual EmailRecordType Type { get; set; }

        [Required]
        public virtual DateTime Timestamp { get; set; }
    }
}
