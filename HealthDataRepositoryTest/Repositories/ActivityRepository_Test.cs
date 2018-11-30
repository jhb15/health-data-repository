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
        public async void AddAsync_AddsToContext()
        {
            var activity = ActivityGenerator.Create();
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                var repository = new ActivityRepository(context);
                await repository.AddAsync(activity);
                Assert.Equal(1, await context.Activity.CountAsync());
                Assert.Equal(activity, await context.Activity.SingleAsync());
            }
        }

        [Fact]
        public async void DeleteAsync_RemovesFromContext()
        {
            var activity = ActivityGenerator.Create();
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.Add(activity);
                context.SaveChanges();
                Assert.Equal(1, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                await repository.DeleteAsync(activity);
                Assert.Equal(0, await context.Activity.CountAsync());
            }
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

        [Fact]
        public async void GetByUserIdAsync_ReturnsCorrectItems()
        {
            var list = ActivityGenerator.CreateList(5);
            list[0].UserId = "someone else";
            var expected = list.GetRange(1, 4);
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                var activities = await repository.GetByUserIdAsync(expected[0].UserId);
                Assert.IsType<List<Activity>>(activities);
                Assert.Equal(expected, activities);
            }
        }

        [Fact]
        public async void GetByUserIdAsync_ReturnsCorrectItemsWithDates()
        {
            var list = ActivityGenerator.CreateList(5);
            list[0].StartTimestamp = new DateTime(2020, 01, 01);
            list[0].EndTimestamp = new DateTime(2020, 01, 02);
            var expected = list.GetRange(1, 4);
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.AddRange(list);
                context.SaveChanges();
                Assert.Equal(list.Count, await context.Activity.CountAsync());
                var repository = new ActivityRepository(context);
                var activities = await repository.GetByUserIdAsync(
                    expected[0].UserId,
                    new DateTime(),
                    new DateTime(2019, 12, 31)
                );
                Assert.IsType<List<Activity>>(activities);
                Assert.Equal(expected, activities);
            }
        }

        [Fact]
        public async void UpdateAsync_UpdatesInContext()
        {
            var activity = ActivityGenerator.Create();
            using (var context = new HealthDataRepositoryContext(contextOptions))
            {
                context.Database.EnsureCreated();
                context.Activity.Add(activity);
                context.SaveChanges();
                var repository = new ActivityRepository(context);
                var newActivity = await repository.GetByIdAsync(activity.Id);
                newActivity.CaloriesBurnt = 5000;
                await repository.UpdateAsync(newActivity);
                Assert.Equal(1, await context.Activity.CountAsync());
                Assert.Equal(newActivity, await context.Activity.SingleAsync());
            }
        }
    }
}
