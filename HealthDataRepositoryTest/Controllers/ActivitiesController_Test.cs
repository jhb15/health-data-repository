using HealthDataRepository.Controllers;
using HealthDataRepository.Models;
using HealthDataRepository.Repositories;
using HealthDataRepositoryTest.TestUtils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HealthDataRepositoryTest.Controllers
{
    public class ActivitiesController_Test
    {
        private readonly Mock<IActivityRepository> activityRepository;
        private readonly Mock<IActivityTypeRepository> activityTypeRepository;
        private readonly ActivitiesController controller;

        public ActivitiesController_Test()
        {
            activityRepository = new Mock<IActivityRepository>();
            activityTypeRepository = new Mock<IActivityTypeRepository>();
            controller = new ActivitiesController(activityRepository.Object, activityTypeRepository.Object);
        }

        [Fact]
        public async void GetActivity_ReturnsNotFoundOnInvalidId()
        {
            activityRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Activity)null).Verifiable();
            var result = await controller.GetActivity(1);
            Assert.IsType<NotFoundResult>(result);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetActivity_ReturnsActivity()
        {
            var activity = ActivityGenerator.Create();
            activityRepository.Setup(r => r.GetByIdAsync(activity.Id)).ReturnsAsync(activity).Verifiable();
            var result = await controller.GetActivity(activity.Id);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<Activity>(content.Value);
            Assert.Equal(activity, content.Value);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetByUserId_ReturnsActivities()
        {
            var activities = ActivityGenerator.CreateList();
            activityRepository.Setup(r => r.GetByUserIdAsync("someuserid")).ReturnsAsync(activities).Verifiable();
            var result = await controller.GetByUserId("someuserid", new DateTime(), new DateTime());
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<List<Activity>>(content.Value);
            Assert.Equal(activities, content.Value);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetByUserId_ReturnsActivitiesFromDates()
        {
            var activities = ActivityGenerator.CreateList();
            var from = new DateTime(2018, 11, 01);
            var to = new DateTime(2018, 11, 30);
            activityRepository.Setup(r => r.GetByUserIdAsync("someuserid", from, to)).ReturnsAsync(activities).Verifiable();
            var result = await controller.GetByUserId("someuserid", from, to);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<List<Activity>>(content.Value);
            Assert.Equal(activities, content.Value);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void PutActivity_ReturnsBadRequest()
        {

            var invalidActivity = new Activity
            {
                Id = 1,
                ActivityTypeId = 5,
                StartTimestamp = new DateTime(2018, 10, 15),
                EndTimestamp = new DateTime(2018, 10, 14),
                Source = "InvalidSource"
            };
            activityTypeRepository.Setup(atr => atr.GetByIdAsync(invalidActivity.ActivityTypeId)).ReturnsAsync((ActivityType)null).Verifiable();

            var result = await controller.PutActivity(0, invalidActivity);
            Assert.IsType<BadRequestObjectResult>(result);
            var content = result as BadRequestObjectResult;
            Assert.IsType<SerializableError>(content.Value);
            var errors = content.Value as SerializableError;

            var expectedErrorKeys = new[] { "Id", "ActivityTypeId", "EndTimestamp", "Source" };

            foreach (var key in expectedErrorKeys)
            {
                Assert.True(errors.ContainsKey(key), $"Should have {key} error");
            }
            activityTypeRepository.Verify();
            activityTypeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void PutActivity_UpdatesActivity()
        {
            var activity = ActivityGenerator.Create();
            var activityType = ActivityTypeGenerator.Create();
            activity.ActivityTypeId = activityType.Id;
            activityTypeRepository.Setup(atr => atr.GetByIdAsync(activity.ActivityTypeId)).ReturnsAsync(activityType).Verifiable();
            activityRepository.Setup(ar => ar.UpdateAsync(activity)).ReturnsAsync(activity).Verifiable();

            var result = await controller.PutActivity(activity.Id, activity);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<Activity>(content.Value);
            Assert.Equal(activity, content.Value);
            activityTypeRepository.Verify();
            activityTypeRepository.VerifyNoOtherCalls();
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void PostActivity_ReturnsBadRequest()
        {

            var invalidActivity = new Activity
            {
                ActivityTypeId = 5,
                StartTimestamp = new DateTime(2018, 10, 15),
                EndTimestamp = new DateTime(2018, 10, 14),
                Source = "InvalidSource"
            };
            activityTypeRepository.Setup(atr => atr.GetByIdAsync(invalidActivity.ActivityTypeId)).ReturnsAsync((ActivityType)null).Verifiable();

            var result = await controller.PostActivity(invalidActivity);
            Assert.IsType<BadRequestObjectResult>(result);
            var content = result as BadRequestObjectResult;
            Assert.IsType<SerializableError>(content.Value);
            var errors = content.Value as SerializableError;

            var expectedErrorKeys = new[] { "ActivityTypeId", "EndTimestamp", "Source" };

            foreach (var key in expectedErrorKeys)
            {
                Assert.True(errors.ContainsKey(key), $"Should have {key} error");
            }
            activityTypeRepository.Verify();
            activityTypeRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void PostActivity_CreatesActivity()
        {
            var activity = ActivityGenerator.Create();
            var activityType = ActivityTypeGenerator.Create();
            activity.ActivityTypeId = activityType.Id;
            activityTypeRepository.Setup(atr => atr.GetByIdAsync(activity.ActivityTypeId)).ReturnsAsync(activityType).Verifiable();
            activityRepository.Setup(ar => ar.AddAsync(activity)).ReturnsAsync(activity).Verifiable();

            var result = await controller.PostActivity(activity);
            Assert.IsType<CreatedAtActionResult>(result);
            var content = result as CreatedAtActionResult;
            Assert.IsType<Activity>(content.Value);
            Assert.Equal(activity, content.Value);
            activityTypeRepository.Verify();
            activityTypeRepository.VerifyNoOtherCalls();
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void DeleteActivity_ReturnsNotFound()
        {
            activityRepository.Setup(ar => ar.GetByIdAsync(1)).ReturnsAsync((Activity)null).Verifiable();

            var result = await controller.DeleteActivity(1);
            Assert.IsType<NotFoundResult>(result);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async void DeleteActivity_RemovesActivity()
        {
            var activity = ActivityGenerator.Create();
            activityRepository.Setup(ar => ar.GetByIdAsync(activity.Id)).ReturnsAsync(activity).Verifiable();
            activityRepository.Setup(ar => ar.DeleteAsync(activity)).ReturnsAsync(activity).Verifiable();

            var result = await controller.DeleteActivity(activity.Id);
            Assert.IsType<OkObjectResult>(result);
            var content = result as OkObjectResult;
            Assert.IsType<Activity>(content.Value);
            Assert.Equal(activity, content.Value);
            activityRepository.Verify();
            activityRepository.VerifyNoOtherCalls();
        }
    }
}
