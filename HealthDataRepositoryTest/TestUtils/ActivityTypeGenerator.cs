using HealthDataRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthDataRepositoryTest.TestUtils
{
    public class ActivityTypeGenerator
    {
        public static ActivityType Create(int index = 0)
        {
            return new ActivityType
            {
                Name = $"Type {index}"
            };
        }

        public static List<ActivityType> CreateList(int length = 5)
        {
            List<ActivityType> list = new List<ActivityType>();
            for (var i = 0; i < length; i++)
            {
                list.Add(Create(i));
            }
            return list;
        }
    }
}
