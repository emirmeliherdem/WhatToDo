using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatToDo.Models
{
    public class RecommendedHabit
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int AimedDayCount { get; set; }

        public int UserCount { get; set; }
    }
}