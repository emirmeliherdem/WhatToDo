using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatToDo.Models
{
    public class Hobby
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public bool IsDone { get; set; }

        public int AimedDayCount { get; set; }

        public int MissedDayCount { get; set; }

        public int CompDayCount { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}