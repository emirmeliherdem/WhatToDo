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

        public string Description { get; set; }

        public bool IsDone { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive value.")]
        public int AimedDayCount { get; set; }

        public int MissedDayCount { get; set; }

        public int CompDayCount { get; set; }

        public virtual ApplicationUser User { get; set; }


        public int getPercentage()
        {
            if (AimedDayCount <= 0)
                return 0;
            return (int) Math.Round(100f * ((float) CompDayCount / (float) AimedDayCount));
        }
    }
}