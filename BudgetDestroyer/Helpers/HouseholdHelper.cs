using BudgetDestroyer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Helpers
{
    public class HouseholdHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserRolesHelper userRolesHelper = new UserRolesHelper();

        public static int? GetUserHouseholdId(string UserId)
        {
            int? householdId = db.Users.Find(UserId).HouseholdId;

            return householdId;
        }

        public static string GetHouseholdName(string UserId)
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
            if (db.Users.AsNoTracking().First(u => u.Id == UserId).HouseholdId > 0)
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

        public static bool AnyUsersInHousehold(int householdId)
        {
            var users = db.Users.Where(u => u.HouseholdId == householdId);

            foreach (var user in users)
            {
                if (userRolesHelper.IsUserInRole(user.Id, "User"))
                {
                    return true;
                }
            }

            return false;
        }

        public static ICollection<ApplicationUser> UsersInHouse(int householdId)
        {
            return db.Users.Where(u => u.HouseholdId == householdId).ToList();
        }

        public static bool AssignRandomHoH(int householdId)
        {
           foreach (var user in HouseholdHelper.UsersInHouse(householdId))
            {
                if (userRolesHelper.IsUserInRole(user.Id, "User"))
                {
                    userRolesHelper.RemoveUserFromRole(user.Id, "User");
                    userRolesHelper.AddUserToRole(user.Id, "HoH");

                    return true;
                }
            }

            return true;
        }
    }
}