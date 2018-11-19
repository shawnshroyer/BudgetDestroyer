using BudgetDestroyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;

namespace BudgetDestroyer.Extensions
{
    public class TransactionAuthorizeAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId = HttpContext.Current.User.Identity.GetUserId();
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData.Remove("oopsMsg");
            if (userId == null)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to continue.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            var householdId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            int? userHouseholdId = db.Users.Find(userId).HouseholdId;

            if (userHouseholdId != householdId)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to view this Household.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class AccountsAuthorizeAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId = HttpContext.Current.User.Identity.GetUserId();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData.Remove("oopsMsg");
            if (userId == null)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to continue.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            var accountId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            int? userHouseholdId = db.Users.Find(userId).HouseholdId;
            var account = db.HouseAccounts.Find(accountId);

            if (userHouseholdId != account.HouseholdId)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to view this Account.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class BudgetsAuthorizeAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId = HttpContext.Current.User.Identity.GetUserId();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData.Remove("oopsMsg");
            if (userId == null)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to continue.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            var budgetId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            int? userHouseholdId = db.Users.Find(userId).HouseholdId;
            var budget = db.Budgets.Find(budgetId);

            if (budget.HouseholdId != userHouseholdId)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to view this Budget.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class BudgetItemsAuthorizeAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId = HttpContext.Current.User.Identity.GetUserId();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData.Remove("oopsMsg");
            if (userId == null)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to continue.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            var budgetItemsId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            int? userHouseholdId = db.Users.Find(userId).HouseholdId;
            var budgetItem = db.BudgetItems.Find(budgetItemsId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);

            if (budget.HouseholdId != userHouseholdId)
            {
                filterContext.Controller.TempData.Add("oopsMsg", "You have to be logged in and authorized to view this Budget Item.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}