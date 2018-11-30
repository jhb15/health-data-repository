using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Models
{
    /**
     *  Class representing a single instance of 
     *  a user's activity. For example, this model
     *  may represent a single Workout which may be 
     *  a jog, swim, bike ride, etc.
    */
    public class Activity
    {

        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string UserId { get; set; }

        [Required]
        public virtual DateTime StartTimestamp { get; set; }

        [Required]
        public virtual DateTime EndTimestamp { get; set; }

        [Required]
        public virtual DataSource Source { get; set; }

        [Required]
        public virtual ActivityType ActivityType { get; set; }

        public virtual int CaloriesBurnt { get; set; }

        public virtual int AverageHeartRate { get; set; }

        public virtual int StepsTaken { get; set; }

        public virtual double MetresTravelled { get; set; }

        public virtual double MetresElevationGained { get; set; }
    }
}
