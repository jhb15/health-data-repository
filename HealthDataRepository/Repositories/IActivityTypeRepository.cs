using HealthDataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Repositories
{
    public interface IActivityTypeRepository
    {
        Task<ActivityType> GetByIdAsync(int id);

        Task<List<ActivityType>> GetAllAsync();
    }
}
