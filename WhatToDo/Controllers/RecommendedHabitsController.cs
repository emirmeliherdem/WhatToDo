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


        // GET: RecommendedHabits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendedHabit recommendedHabit = db.RecommendedHabits.Find(id);
            if (recommendedHabit == null)
            {
                return HttpNotFound();
            }
            return View(recommendedHabit);
        }

        public ActionResult AJAXCreate(Habit habit)
        {
            // create recommended habit & add to database
            RecommendedHabit recHabit = new RecommendedHabit();
            recHabit.Description = habit.Description;
            recHabit.UserCount = 0;
            db.RecommendedHabits.Add(recHabit);
            db.SaveChanges();
            return RedirectToAction("Index", "Habits");
        }

        // POST: RecommendedHabits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,UserCount")] RecommendedHabit recommendedHabit)
        {
            if (ModelState.IsValid)
            {
                db.RecommendedHabits.Add(recommendedHabit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recommendedHabit);
        }

        // GET: RecommendedHabits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendedHabit recommendedHabit = db.RecommendedHabits.Find(id);
            if (recommendedHabit == null)
            {
                return HttpNotFound();
            }
            return View(recommendedHabit);
        }

        // POST: RecommendedHabits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,UserCount")] RecommendedHabit recommendedHabit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recommendedHabit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recommendedHabit);
        }

        // GET: RecommendedHabits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendedHabit recommendedHabit = db.RecommendedHabits.Find(id);
            if (recommendedHabit == null)
            {
                return HttpNotFound();
            }
            return View(recommendedHabit);
        }

        // POST: RecommendedHabits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecommendedHabit recommendedHabit = db.RecommendedHabits.Find(id);
            db.RecommendedHabits.Remove(recommendedHabit);
            db.SaveChanges();
            return RedirectToAction("Index");
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
