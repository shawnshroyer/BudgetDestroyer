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
    public class HouseAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: HouseAccounts
        //public ActionResult Index()
        //{
        //    var HouseAccounts = db.HouseAccounts.Include(h => h.Household);
        //    return View(HouseAccounts.ToList());
        //}

        //// GET: HouseAccounts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HouseAccount houseAccount = db.HouseAccounts.Find(id);
        //    if (houseAccount == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(houseAccount);
        //}

        //// GET: HouseAccounts/Create
        //public ActionResult Create()
        //{
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
        //    return View();
        //}

        // POST: HouseAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Balance,ReconciledBalace")] HouseAccount houseAccount)
        {
            if (ModelState.IsValid)
            {
                houseAccount.HouseholdId = HouseholdHelper.GetUserHouseholdId(User.Identity.GetUserId()).Value;
                houseAccount.ReconciledBalace = 0.00M;

                db.HouseAccounts.Add(houseAccount);
                db.SaveChanges();
                return RedirectToAction("Index", "Households");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", houseAccount.HouseholdId);
            return View(houseAccount);
        }

        // GET: HouseAccounts/Edit/5
        [AccountsAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HouseAccount houseAccount = db.HouseAccounts.Find(id);
            if (houseAccount == null)
            {
                return HttpNotFound();
            }
            //ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", houseAccount.HouseholdId);
            return View(houseAccount);
        }

        // POST: HouseAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,Balance,ReconciledBalace")] HouseAccount houseAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(houseAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", houseAccount.HouseholdId);
            return View(houseAccount);
        }

        //// GET: HouseAccounts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HouseAccount houseAccount = db.HouseAccounts.Find(id);
        //    if (houseAccount == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(houseAccount);
        //}

        // POST: HouseAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [AccountsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseAccount houseAccount = db.HouseAccounts.Find(id);
            db.HouseAccounts.Remove(houseAccount);
            db.SaveChanges();
            return RedirectToAction("Index", "Households");
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
