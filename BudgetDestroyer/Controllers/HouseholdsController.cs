﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BudgetDestroyer.Models;
using BudgetDestroyer.Extensions;
using BudgetDestroyer.Helpers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace BudgetDestroyer.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper userRolesHelper = new UserRolesHelper();

        // GET: Households
        public ActionResult Index()
        {
            var householdId = HouseholdHelper.GetUserHouseholdId(User.Identity.GetUserId());

            ViewBag.HouseAccounts = db.HouseAccounts.Where(h => h.HouseholdId == householdId).ToList();
            ViewBag.Transactions = db.Transactions.Where(t => db.HouseAccounts.Any(h =>
                h.Id == t.HouseAccountId && h.HouseholdId == householdId)).ToList();

            ViewBag.HouseAccountsSelect = new SelectList(ViewBag.HouseAccounts, "Id", "Name");
            ViewBag.TransactionTypes = new SelectList(db.TransactionTypes, "Id", "Name");

            ViewBag.BudgetItems = db.BudgetItems.Where(i => db.Budgets.Any(b => b.Id == i.BudgetId && b.HouseholdId == householdId)).ToList();
            ViewBag.BudgetItemSelect= new SelectList(ViewBag.BudgetItems, "Id", "Name");

            ViewBag.Budgets = db.Budgets.Where(b => b.HouseholdId == householdId).ToList();
            ViewBag.BudgetId = new SelectList(ViewBag.Budgets, "Id", "Name");

            return View(db.Households.Find(householdId));
        }

        //// GET: Households/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();

                HouseholdHelper.AddUserToHouse(User.Identity.GetUserId(), household.Id);
                userRolesHelper.AddUserToRole(User.Identity.GetUserId(), "HoH");

                return RedirectToAction("Index");
            }

            return View(household);
        }

        //// GET: Households/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        //// POST: Households/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(household).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(household);
        //}

        //// GET: Households/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        //// POST: Households/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Household household = db.Households.Find(id);
        //    db.Households.Remove(household);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // POST: Households/Invitation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invitation(string Email)
        {
            try
            {
                var email = new MailAddress(Email).ToString();

                foreach (var invite in db.Invitations.Where(i => i.EmailTo.ToLower() == email.ToLower()))
                {
                    if (invite.Accepted)
                    {
                        TempData["status"] = "accepted";
                        return RedirectToAction("Index", "Households", null);
                    }

                    if (invite.Expires >= DateTime.Now)
                    {
                        TempData["status"] = "pending";
                        return RedirectToAction("Index", "Households", null);
                    }
                }

                Invitation invitation = new Invitation
                {
                    Created = DateTime.Now,
                    Expires = DateTime.Now.AddDays(3),
                    EmailTo = email,
                    Subject = $"{User.Identity.FullName()} has invited you to join Budget Destoyer",
                    Body = $"{User.Identity.FullName()} has invited you to join their house {HouseholdHelper.GetHouseholdName(User.Identity.GetUserId())} on Budget Destroyer",
                    HouseholdId = Convert.ToInt32(HouseholdHelper.GetUserHouseholdId(User.Identity.GetUserId())),
                    UniqueCode = Guid.NewGuid(),
                    Accepted = false
                };

                db.Invitations.Add(invitation);
                db.SaveChanges();

                string code = invitation.UniqueCode.ToString();
                var callbackUrl = Url.Action("RegisterInvitation", "Account", new { code = code }, protocol: Request.Url.Scheme);

                //var message = "<p>Email From: <bold>{0}</bold>({1})</p><p> Message:</p><p>{2}</p> ";
                var bodyButton = "<a href =\"" + callbackUrl + "\">here</a>";
                var sentEmail = new MailMessage("noreply@gmail.com", email)
                {
                    Subject = invitation.Subject,
                    Body = $"{invitation.Body}. Please Join by clicking {bodyButton}",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(sentEmail);

                TempData["status"] = "success";
                return RedirectToAction("Index", "Households", null);
            }
            catch (FormatException ex)
            {
                TempData["status"] = "error";
                return RedirectToAction("Index", "Households", null);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Leaving()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var household = db.Households.Find(user.HouseholdId);

            if (User.IsInRole("User"))
            {
                userRolesHelper.RemoveUserFromRole(userId, "User");
                user.HouseholdId = null;

                db.SaveChanges();
            }

            if (userRolesHelper.IsUserInRole(userId, "HoH"))
            {
                if (HouseholdHelper.AnyUsersInHousehold(household.Id))
                {
                    HouseholdHelper.AssignRandomHoH(household.Id);
                }

                userRolesHelper.RemoveUserFromRole(userId, "HoH");
                user.HouseholdId = null;

                db.Households.Remove(household);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
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
