﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace health_data_repository.Models
{
    /**
     *  Class representing a single instance of 
     *  a user's activity. For example, this model
     *  may represent a single Workout which may be 
     *  a jog, swim, bike ride, etc.
    */
    public class Activity
    {
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

        /**
         * TODO: Needs considering, activities such as cycling 
         * obviously won't have steps.
        */
        public virtual int StepsTaken { get; set; }

        /**
         *  Distance travelled during this activity,
         *  measured in kilometers
         *  
         *  TODO: Check this. FitBit API says KM is default, 
         *  but probably needs discussing
        */
        public virtual float DistanceTravelled { get; set; }

        /**
         * Elevation gained during this activity, measured in
         * metres.
         * 
         * TODO: Check units against FitBit API responses
        */
        public virtual float ElevationGained { get; set; }
    }
}
