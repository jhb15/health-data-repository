using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    public enum DataSource
    {
        Manual,
        Fitbit
    }

    public static class DataSourceMethods
    {
        public static string GetValuesAsArrayString(this DataSource _)
        {
            var values = string.Join(", ", Enum.GetNames(typeof(DataSource)));
            return $"[{values}]";
        }
    }
}
