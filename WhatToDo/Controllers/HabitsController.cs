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
                if (habit.CompDayCount <= habit.AimedDayCount)
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
                }
            }
            db.SaveChanges();
        }


        public ActionResult BuildHabitTable()
        {
            return PartialView("_HabitTable", GetHabits());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] Habit habit)
        {
            if (ModelState.IsValid && habit.Description != null)
            {
                string currentUserId = User.Identity.GetUserId();
                // find the user with the given user id
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                // match current to do with the user
                habit.User = currentUser;
                habit.IsDone = false;
                habit.AimedDayCount = 0;
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
