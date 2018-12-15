using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthDataRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDataRepository.Repositories
{
    public class ActivityTypeRepository : IActivityTypeRepository
    {
        private readonly HealthDataRepositoryContext context;

        public ActivityTypeRepository(HealthDataRepositoryContext context)
        {
            this.context = context;
        }

        public async Task<List<ActivityType>> GetAllAsync()
        {
            return await context.ActivityType.ToListAsync();
        }

        public async Task<ActivityType> GetByIdAsync(int id)
        {
            return await context.ActivityType.FindAsync(id);
        }
    }
}
