using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthDataRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDataRepository.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly HealthDataRepositoryContext context;

        public ActivityRepository(HealthDataRepositoryContext context)
        {
            this.context = context;
        }

        public async Task<Activity> AddAsync(Activity activity)
        {
            context.Activity.Add(activity);
            await context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> DeleteAsync(Activity activity)
        {
            context.Activity.Remove(activity);
            await context.SaveChangesAsync();
            return activity;
        }

        public async Task<PaginatedList<Activity>> GetAllPaginatedAsync(int pageNumber, int perPage)
        {
            var source = context.Activity.Include(a => a.ActivityType).OrderByDescending(a => a.StartTimestamp);
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * perPage).Take(perPage).ToListAsync();
            return new PaginatedList<Activity>(items, count, pageNumber, perPage);
        }

        public async Task<Activity> GetByIdAsync(int id)
        {
            return await context.Activity.FindAsync(id);
        }

        public async Task<List<Activity>> GetByUserIdAsync(string userId)
        {
            return await context.Activity.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<List<Activity>> GetByUserIdAsync(string userId, DateTime from, DateTime to)
        {
            return await context.Activity
                .Where(e => e.UserId == userId)
                .Where(e => e.StartTimestamp >= from)
                .Where(e => e.EndTimestamp <= to)
                .ToListAsync();
        }

        public async Task<List<string>> GetUsersWithRecordedActivities()
        {
            return await context.Activity.Select(a => a.UserId).Distinct().ToListAsync();
        }

        public async Task<Activity> UpdateAsync(Activity activity)
        {
            context.Entry(activity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return activity;
        }
    }
}
