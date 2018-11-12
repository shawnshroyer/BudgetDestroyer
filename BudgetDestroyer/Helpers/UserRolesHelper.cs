using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BudgetDestroyer.Models;
using System.Web.Mvc;

namespace BudgetDestroyer.Helpers
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }

            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }

            return resultList;
        }

        public ICollection<ApplicationUser> ListAllUsers()
        {
            var resultList = new List<ApplicationUser>(userManager.Users.ToList());

            return resultList;
        }

        public SelectList ListAllRoles()
        {
            var resultList = new SelectList(db.Roles.ToList(), "Id", "Name");

            return resultList;
        }

        //Used to update data in the user profile. 
        //Would like to add return for success or failure
        public void UpdateUserData(ChangeUserProfileViewModel user)
        {
            var thisUser = db.Users.Find(user.Id);

            thisUser.Id = user.Id;
            thisUser.FirstName = user.FirstName;
            thisUser.LastName = user.LastName;
            thisUser.DisplayName = user.DisplayName;
            thisUser.AvatarPath = user.AvatarPath;
            thisUser.Email = user.Email;
            thisUser.UserName = user.Email;

            db.Users.Attach(thisUser);
            db.Entry(thisUser).Property(x => x.FirstName).IsModified = true;
            db.Entry(thisUser).Property(x => x.LastName).IsModified = true;
            db.Entry(thisUser).Property(x => x.DisplayName).IsModified = true;
            db.Entry(thisUser).Property(x => x.AvatarPath).IsModified = true;
            db.Entry(thisUser).Property(x => x.Email).IsModified = true;
            db.Entry(thisUser).Property(x => x.UserName).IsModified = true;

          
            db.SaveChanges();
        }

        public string GetUserAvatar(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(userManager.FindById(userId).AvatarPath))
            {
                return string.Empty;
            }

            return userManager.FindById(userId).AvatarPath.ToString();
        }

        public string GetUserRole(string userId)
        {
            foreach(var role in userManager.GetRoles(userId))
            {
                return role;
            }

            return "User Not In Role";
        }
    }
}