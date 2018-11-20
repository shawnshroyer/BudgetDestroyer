using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetDestroyer.Extensions;
using BudgetDestroyer.Helpers;
using BudgetDestroyer.Models;
using Microsoft.AspNet.Identity;

namespace BudgetDestroyer.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
                private ApplicationDbContext db = new ApplicationDbContext();

        //        // GET: Budgets
        //        public ActionResult Index()
        //        {
        //            var budgets = db.Budgets.Include(b => b.Household);
        //            return View(budgets.ToList());
        //        }

        //        // GET: Budgets/Details/5
        //        public ActionResult Details(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            Budget budget = db.Budgets.Find(id);
        //            if (budget == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            return View(budget);
        //        }

        //        // GET: Budgets/Create
        //        public ActionResult Create()
        //        {
        //            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
        //            return View();
        //        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Descriptions,Amount")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                budget.HouseholdId = HouseholdHelper.GetUserHouseholdId(User.Identity.GetUserId()).Value;

                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index", "Households");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        [BudgetsAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,Descriptions,Amount")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Households");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        //        // GET: Budgets/Delete/5
        //        public ActionResult Delete(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            Budget budget = db.Budgets.Find(id);
        //            if (budget == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            return View(budget);
        //        }

        //// POST: Budgets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Budget budget = db.Budgets.Find(id);
        //    db.Budgets.Remove(budget);
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "Households");
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
