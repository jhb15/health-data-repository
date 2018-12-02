using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthDataRepository.Models;

namespace HealthDataRepository.Models
{
    public class HealthDataRepositoryContext : DbContext
    {
        public HealthDataRepositoryContext (DbContextOptions<HealthDataRepositoryContext> options)
            : base(options)
        {
        }

        public DbSet<HealthDataRepository.Models.Activity> Activity { get; set; }

        public DbSet<HealthDataRepository.Models.ActivityType> ActivityType { get; set; }

        public DbSet<HealthDataRepository.Models.ActivityMapping> ActivityMapping { get; set; }
    }
}
