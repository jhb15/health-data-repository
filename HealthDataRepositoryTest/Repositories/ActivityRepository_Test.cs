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
    public class ActivityRepository_Test
    {
        private static readonly Random random = new Random();
        private readonly DbContextOptions<HealthDataRepositoryContext> contextOptions;

        public ActivityRepository_Test()
        {
            contextOptions = new DbContextOptionsBuilder<HealthDataRepositoryContext>()
                .UseInMemoryDatabase($"rand_db_name_{random.Next()}")
                .Options;
        }

        [Fact]
        public async void GetByIdAsync_ReturnsCorrectItems()
        {
            var list = ActivityGenerator.CreateList(5);
            var expected = list[2];
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                var activity = await repository.GetByIdAsync(expected.Id);
                Assert.IsType<Activity>(activity);
                Assert.Equal(expected, activity);
            }
        }
    }
}
