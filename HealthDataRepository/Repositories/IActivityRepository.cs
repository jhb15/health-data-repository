using HealthDataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Repositories
{
    public interface IActivityRepository
    {
        Task<PaginatedList<Activity>> GetAllPaginatedAsync(int pageNumber, int perPage);

        Task<List<Activity>> GetByUserIdAsync(string userId);

        Task<List<Activity>> GetByUserIdAsync(string userId, DateTime from, DateTime to);

        Task<Activity> GetByIdAsync(int id);

        Task<Activity> AddAsync(Activity activity);

        Task<Activity> UpdateAsync(Activity activity);

        Task<Activity> DeleteAsync(Activity activity);
    }
}
