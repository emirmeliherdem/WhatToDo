using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatToDo.Models
{
    public class Habit
    {
        private const int HISTORY_SIZE = 5;

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

        // 0 = empty, 1 = miss, 2 = done
        public string History { get; set; } = "";

        public virtual ApplicationUser User { get; set; }


        // Returns the completion percentage of the habit
        public int GetPercentage()
        {
            if (AimedDayCount <= 0)
                return 0;
            return (int) Math.Round(100f * ((float) CompDayCount / (float) AimedDayCount));
        }

        // Returns the miss percentage of the habit in relation to completed days
        public int GetMissPercentage()
        {
            return Percentage(MissedDayCount, CompDayCount);
        }

        // Returns the average miss percentage of the habit compared to other users following the same habit
        public int GetAvgMissPercentage()
        {
            if (RecId < 0)
                return -1;
            else
                return Controllers.HabitsController.GetAvgMissPercentage(RecId);
        }

        // Returns whether the aimed day count is reached or not
        public bool IsComplete()
        {
            return CompDayCount >= AimedDayCount;
        }

        public void Done()
        {
            CompDayCount++;
            History += 2; // add 'done' (2) to habit history
            if (History.Length > HISTORY_SIZE)
                History = History.Substring(1);
            IsDone = false;
        }

        public void Miss()
        {
            MissedDayCount++;
            History += 1; // add 'miss' (1) to habit history
            if (History.Length > HISTORY_SIZE)
                History = History.Substring(1);
        }

        // Returns true if the habit is one miss away from being deleted (miss percentage >= 100)
        public bool InDanger()
        {
            return Percentage(MissedDayCount + 1, CompDayCount) >= 100;
        }

        private int Percentage(int n1, int n2)
        {
            if (n1 <= 0 && n2 <= 0)
                return 0;
            else if (n2 <= 0)
                return n1 * 100;

            return (int)Math.Round(100f * ((float)n1 / (float)n2));
        }
    }
}