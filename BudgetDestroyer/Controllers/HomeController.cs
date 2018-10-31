using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetDestroyer.Models;
using BudgetDestroyer.Extensions;
using Microsoft.AspNet.Identity;

namespace BudgetDestroyer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // If Index is used for Appdetails page then uncomment
            //if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Households");
            //}
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserProfile()
        {
            ViewBag.Profile = new ApplicationUser();
            ViewBag.Password = new ChangePasswordViewModel();

            ViewBag.Profile.Id = User.Identity.GetUserId();
            ViewBag.Profile.FirstName = User.Identity.FirstName();
            ViewBag.Profile.LastName = User.Identity.LastName();
            ViewBag.Profile.DisplayName = User.Identity.DisplayName();
            ViewBag.Profile.AvatarPath = User.Identity.AvatarPath();
            ViewBag.Profile.Email = User.Identity.GetUserName();
            ViewBag.Profile.UserName = User.Identity.GetUserName();

            return View();
        }
    }
}