using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetDestroyer.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace BudgetDestroyer.Controllers
{
    [Authorize]
    public class MorrisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetBudgetDataForBarChart()
        {
            var budgetData = new List<MorrisBudgetBar>();
            var userId = User.Identity.GetUserId();
            var houseId = db.Users.Find(userId).HouseholdId;
            var budgets = db.Households.Find(houseId).Budgets.ToList();
            var budgetItems = db.BudgetItems.Where(i => db.Budgets.Any(b => b.Id == i.BudgetId && b.HouseholdId == houseId)).ToList();

            if (houseId == null)
            {
                return Content(JsonConvert.SerializeObject(budgetData), "application/json");
            }

            

            foreach (var budget in budgets) {
                foreach(var item in budgetItems)
                {

                }
            }

            //var transactions = db.Transactions.Include(t => t.EnteredBy).Include(t => t.HouseAccount).Include(t => t.TransactionType);

            foreach (var budget in budgets)
            {
                var temp = db.Transactions.Where(t => db.BudgetItems.Any(i => i.Id == t.BudgetItemId && budget.Id == i.BudgetId));
                decimal amount = 0;
                foreach (var tempTran in temp)
                {
                    if (tempTran.Amount < 0)
                    {
                        amount += tempTran.Amount;
                    }
                }
                amount *= -1;

                budgetData.Add(new MorrisBudgetBar
                {
                    Label = budget.Name,
                    Target = (budget.Amount + amount),
                    Actual = budget.Amount
                });
            }

            return Content(JsonConvert.SerializeObject(budgetData), "application/json");
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
