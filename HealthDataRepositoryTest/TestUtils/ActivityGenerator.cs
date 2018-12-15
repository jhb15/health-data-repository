using HealthDataRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthDataRepositoryTest.TestUtils
{
    public class ActivityGenerator
    {
        public static Activity Create(int index = 0)
        {
            return new Activity
            {
                ActivityTypeId = 0,
                UserId = "05e7493a-f9a6-4ead-aadf-ff3f964368f3",
                Source = "Manual",
                StartTimestamp = new DateTime(2018, 11, 30, 8, 0, 0),
                EndTimestamp = new DateTime(2018, 11, 30, 9, 0, 0),
                CaloriesBurnt = 100
            };
        }

        public static List<Activity> CreateList(int length = 5)
        {
            List<Activity> list = new List<Activity>();
            for (var i = 0; i < length; i++)
            {
                list.Add(Create(i));
            }
            return list;
        }
    }
}
