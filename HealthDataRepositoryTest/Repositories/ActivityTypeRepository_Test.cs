using HealthDataRepository.Models;
using HealthDataRepository.Repositories;
using HealthDataRepositoryTest.TestUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HealthDataRepositoryTest.Repositories
{
    public class ActivityTypeRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<HealthDataRepositoryContext> contextOptions;

        public ActivityTypeRepository_Test()
        {
            contextOptions = new DbContextOptionsBuilder<HealthDataRepositoryContext>()
                .UseInMemoryDatabase($"rand_db_name_{random.Next()}")
                .Options;
        }

        [Fact]
        public async void GetByIdAsync_ReturnsCorrectItems()
        {
            var list = ActivityTypeGenerator.CreateList(5);
            var expected = list[2];
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.ActivityType.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.ActivityType.CountAsync());
                var repository = new ActivityTypeRepository(context);
                var activityType = await repository.GetByIdAsync(expected.Id);
                Assert.IsType<ActivityType>(activityType);
                Assert.Equal(expected, activityType);
            }
        }
    }
}
