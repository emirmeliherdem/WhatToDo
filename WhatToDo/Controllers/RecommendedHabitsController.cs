using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhatToDo.Models;

namespace WhatToDo.Controllers
{
    public class RecommendedHabitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RecommendedHabits
        public ActionResult Index()
        {
            return View(db.RecommendedHabits.ToList());
        }

        private IEnumerable<RecommendedHabit> GetRecommendedHabits()
        {
            return db.RecommendedHabits.ToList();
        }

        public ActionResult BuildRecHabitTable()
        {
            return PartialView("_RecHabitTable", GetRecommendedHabits());
        }

        public ActionResult Create(Habit habit)
        {
            // create recommended habit & add to database
            RecommendedHabit recHabit = new RecommendedHabit();
            recHabit.Description = habit.Description;
            recHabit.AimedDayCount = habit.AimedDayCount;
            recHabit.UserCount = 0;
            db.RecommendedHabits.Add(recHabit);
            db.SaveChanges();

            habit.RecId = recHabit.Id;
            db.Entry(habit).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Habits");
        }

        public ActionResult FollowRecomendation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendedHabit recHabit = db.RecommendedHabits.Find(id);
            if (recHabit == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("FollowRecomendation", "Habits", recHabit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
