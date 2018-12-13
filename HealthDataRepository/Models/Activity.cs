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
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        [Display(Name = "User")]
        public virtual string UserId { get; set; }

        [Required]
        [Display(Name = "Start")]
        public virtual DateTime StartTimestamp { get; set; }

        [Required]
        [Display(Name = "End")]
        public virtual DateTime EndTimestamp { get; set; }

        [Required]
        public virtual string Source { get; set; }

        [Required]
        public virtual int ActivityTypeId { get; set; }

        [Display(Name = "Type")]
        public virtual ActivityType ActivityType { get; set; }

        [Range(0, 10000)]
        [Display(Name = "Calories")]
        public virtual int CaloriesBurnt { get; set; }

        [Range(0, 300)]
        [Display(Name = "HR")]
        public virtual int AverageHeartRate { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Steps")]
        public virtual int StepsTaken { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Distance")]
        public virtual double MetresTravelled { get; set; }

        [Range(-9000, 9000)]
        [Display(Name = "Elevation")]
        public virtual double MetresElevationGained { get; set; }
    }
}
