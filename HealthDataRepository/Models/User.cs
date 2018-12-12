using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public class User
    {
        public virtual string Id { get; set; }
        public virtual string Email { get; set; }
    }
}
