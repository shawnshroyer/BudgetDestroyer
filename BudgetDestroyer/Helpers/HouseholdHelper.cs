using BudgetDestroyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Helpers
{
    public class HouseholdHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static int? GetUserHouseholdId(string UserId)
        {
            int? householdId = db.Users.Find(UserId).HouseholdId;

            return householdId;
        }

        public static string GetHouseholdName (string UserId)
        {
            int? householdId = db.Users.Find(UserId).HouseholdId;

            if (householdId is null)
            {
                return "Not Found";
            }

            return db.Households.Find(householdId).Name;
        }

        //public bool AddUserToHouse (string UserId)
        //{
        //    int? householdId = db.Users.Find(UserId).HouseholdId;

        //    db.Households.Add(household);
        //    db.SaveChanges();

        //    return false;
        //}

    }
}