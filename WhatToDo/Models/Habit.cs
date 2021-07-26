using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatToDo.Models
{
    public class Habit
    {
        public int Id { get; set; }

        // for recommended habits
        public int RecId { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDone { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive value.")]
        public int AimedDayCount { get; set; }

        public int MissedDayCount { get; set; }
            
        public int CompDayCount { get; set; }

        public virtual ApplicationUser User { get; set; }


        // Returns the completion percentage of the habit
        public int getPercentage()
        {
            if (AimedDayCount <= 0)
                return 0;
            return (int) Math.Round(100f * ((float) CompDayCount / (float) AimedDayCount));
        }

        // Returns the miss percentage of the habit in relation to completed days
        public int getMissPercentage()
        {
            if (MissedDayCount <= 0 && CompDayCount <= 0)
                return 0;
            else if (CompDayCount <= 0)
                return MissedDayCount * 100;

            return (int) Math.Round(100f * ((float) MissedDayCount / (float) CompDayCount));
        }

        // Returns the average miss percentage of the habit compared to other users following the same habit
        public int getAvgMissPercentage()
        {
            if (RecId < 0)
                return -1;
            else
                return Controllers.HabitsController.GetAvgMissPercentage(RecId);
        }

        // Returns whether the aimed day count is reached or not
        public bool isComplete()
        {
            return CompDayCount >= AimedDayCount;
        }
    }
}