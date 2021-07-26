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
    public class HabitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Habits
        public ActionResult Index()
        {
            return View();
        }

        private IEnumerable<Habit> GetHabits()
        {
            string currentUserId = User.Identity.GetUserId();
            // find the user with the given user id
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.Habits.ToList().Where(x => x.User == currentUser);
        }

        // update the day counts of each habit
        public static void UpdateHabits()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<Habit> habits = db.Habits.ToList();
            foreach (var habit in habits)
            {
                // update day counts if the habit is not already complete
                if (!habit.isComplete())
                {
                    if (habit.IsDone)
                    {
                        habit.CompDayCount++;
                        habit.IsDone = false;
                    }
                    else
                    {
                        habit.MissedDayCount++;
                    }
                    db.Entry(habit).State = EntityState.Modified;

                    // remove habit if its miss percentage is over %100
                    if (habit.getMissPercentage() >= 100)
                    {
                        db.Habits.Remove(habit);
                    }
                }
            }
            db.SaveChanges();

            // update recommended habits' average miss percentages
            IEnumerable<RecommendedHabit> recHabits = db.RecommendedHabits.ToList();
            foreach (var recHabit in recHabits)
            {
                int recId = recHabit.Id;
                int sum = 0;
                int count = 0;
                foreach (var habit in habits)
                {
                    if (habit.RecId == recId)
                    {
                        sum += habit.getMissPercentage();
                        count++;
                    }
                }
                // compute averaege miss percentage of the recommended habit
                if (count == 0)
                    recHabit.AvgMissPercentage = 0;
                else
                    recHabit.AvgMissPercentage = (int) Math.Round((float) sum / (float) count);
                db.Entry(recHabit).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        // Returns the average miss percentage of the recommended habit specified by its recId
        public static int GetAvgMissPercentage(int recId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            // find recommended habit by id
            RecommendedHabit recHabit = db.RecommendedHabits.FirstOrDefault(x => x.Id == recId);

            return recHabit.AvgMissPercentage;

        }

        public ActionResult FollowRecomendation(RecommendedHabit recHabit)
        {
            foreach (var habit in GetHabits())
            {
                if (habit.RecId == recHabit.Id)
                {
                    TempData["ErrorMessage"] = "Already following the habit.";
                    return RedirectToAction("Index");
                }
            }

            // else
            // update user count of the recommended habit
            recHabit.UserCount++;
            db.Entry(recHabit).State = EntityState.Modified;

            // find the user
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            // create & add new habit according to the recommended habit
            Habit newHabit = new Habit();
            newHabit.User = currentUser;
            newHabit.RecId = recHabit.Id;
            newHabit.Description = recHabit.Description;
            newHabit.IsDone = false;
            newHabit.AimedDayCount = recHabit.AimedDayCount;
            newHabit.MissedDayCount = 0;
            newHabit.CompDayCount = 0;
            db.Habits.Add(newHabit);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult BuildHabitTable()
        {
            return PartialView("_HabitTable", GetHabits());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description,AimedDayCount")] Habit habit)
        {
            if (ModelState.IsValid && habit.Description != null)
            {
                string currentUserId = User.Identity.GetUserId();
                // find the user with the given user id
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                // match current habit with the user
                habit.User = currentUser;
                habit.RecId = -1;
                habit.IsDone = false;
                habit.MissedDayCount = 0;
                habit.CompDayCount = 0;
                db.Habits.Add(habit);
                db.SaveChanges();

                ModelState.Clear();
            }

            return PartialView("_HabitTable", GetHabits());
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habit habit = db.Habits.Find(id);
            if (habit == null)
            {
                return HttpNotFound();
            }

            // set IsDone value to the given bool value & update database
            habit.IsDone = value;
            db.Entry(habit).State = EntityState.Modified;
            db.SaveChanges();
            return PartialView("_HabitTable", GetHabits());
        }

        [HttpPost]
        public ActionResult AJAXDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habit habit = db.Habits.Find(id);
            if (habit == null)
            {
                return HttpNotFound();
            }

            db.Habits.Remove(habit);
            db.SaveChanges();
            return PartialView("_HabitTable", GetHabits());
        }

        public ActionResult Recommend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habit habit = db.Habits.Find(id);
            if (habit == null)
            {
                return HttpNotFound();
            }

            if (habit.RecId < 0) // not previously recommended
            {
                db.Entry(habit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "RecommendedHabits", habit);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ClearAll()
        {
            IEnumerable<Habit> habits = GetHabits();
            foreach (var habit in habits)
            {
                db.Habits.Remove(habit);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// GET: Habits/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Habit habit = db.Habits.Find(id);
        //    if (habit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(habit);
        //}

        //// GET: Habits/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Habits/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Description,IsDone,AimedDayCount,MissedDayCount,CompDayCount")] Habit habit)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Habits.Add(habit);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(habit);
        //}

        //// GET: Habits/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Habit habit = db.Habits.Find(id);
        //    if (habit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(habit);
        //}

        //// POST: Habits/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Description,IsDone,AimedDayCount,MissedDayCount,CompDayCount")] Habit habit)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(habit).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(habit);
        //}

        //// GET: Habits/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Habit habit = db.Habits.Find(id);
        //    if (habit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(habit);
        //}

        //// POST: Habits/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Habit habit = db.Habits.Find(id);
        //    db.Habits.Remove(habit);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
