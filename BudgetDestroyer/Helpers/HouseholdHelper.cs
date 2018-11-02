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

        public static bool IsUserInAHouse(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            if(db.Users.Find(UserId).HouseholdId > 0)
            {
                return true;
            }

            return false;
        }

        public static bool AddUserToHouse(string UserId, int HouseId)
        {
            var thisUser = db.Users.FirstOrDefault(u => u.Id == UserId);

            thisUser.HouseholdId = HouseId;

            db.Users.Attach(thisUser);
            db.Entry(thisUser).Property(x => x.HouseholdId).IsModified = true;

            db.SaveChanges();

            return true;
        }

    }
}